using Business.ViewModels.PaymentViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Business.Services.PaymentService;

public class StripePaymentGatewayService : IPaymentGatewayService
{
    private readonly StripeClient _client;
    private readonly ILogger<StripePaymentGatewayService> _logger;

    public StripePaymentGatewayService(IConfiguration config, ILogger<StripePaymentGatewayService> logger)
    {
        _client = new StripeClient(config["Stripe:SecretKey"]);
        _logger = logger;
    }

    public async Task<GatewayPaymentResponse> ProcessPaymentAsync(PaymentRequest request)
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(request.Amount * 100),
                Currency = request.Currency,
                Description = request.Description,
                Metadata = request.Metadata ?? new Dictionary<string, string>(),
                ReceiptEmail = request.CustomerEmail
            };

            var service = new PaymentIntentService(_client);
            var intent = await service.CreateAsync(options);

            return new GatewayPaymentResponse(
                intent.Status == "succeeded",
                intent.Id,
                intent.ClientSecret,
                intent.Status,
                DateTime.UtcNow);
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe payment failed");
            return new GatewayPaymentResponse(false, null, null, "failed", DateTime.UtcNow, ex.Message);
        }
    }

    public async Task<GatewayVerificationResponse> VerifyPaymentAsync(string transactionId)
    {
        try
        {
            var service = new PaymentIntentService(_client);
            var intent = await service.GetAsync(transactionId);

            return new GatewayVerificationResponse(
                intent.Status == "succeeded",
                intent.Amount / 100m,
                intent.Currency,
                intent.Status,
                intent.Created);
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, $"Stripe verification failed for {transactionId}");
            return new GatewayVerificationResponse(false, 0, string.Empty, "failed", DateTime.MinValue);
        }
    }
    
    public async Task<PaymentIntentResult> CreatePaymentIntentAsync(string paymentMethodId, long amount, string currency = "usd")
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                PaymentMethod = paymentMethodId,
                Confirm = true,
                ConfirmationMethod = "manual",
                UseStripeSdk = true
            };

            var service = new PaymentIntentService(_client);
            var intent = await service.CreateAsync(options);
            
            if (intent.Status == "requires_action" || intent.Status == "requires_source_action")
            {
                // Card requires authentication
                return new PaymentIntentResult
                {
                    Success = false,
                    RequiresAction = true,
                    ClientSecret = intent.ClientSecret,
                    PaymentIntentId = intent.Id
                };
            }
            else if (intent.Status == "succeeded")
            {
                // Payment succeeded
                return new PaymentIntentResult
                {
                    Success = true,
                    RequiresAction = false,
                    ClientSecret = intent.ClientSecret,
                    PaymentIntentId = intent.Id
                };
            }
            else
            {
                // Invalid status
                _logger.LogWarning($"Unexpected payment intent status: {intent.Status}");
                return new PaymentIntentResult
                {
                    Success = false,
                    RequiresAction = false,
                    ErrorMessage = $"Payment failed with status: {intent.Status}"
                };
            }
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Failed to create payment intent");
            return new PaymentIntentResult
            {
                Success = false,
                RequiresAction = false,
                ErrorMessage = ex.StripeError?.Message ?? ex.Message
            };
        }
    }

    public async Task<GatewayRefundResponse> RefundPaymentAsync(string transactionId, decimal amount)
    {
        try
        {
            var service = new RefundService(_client);
            var options = new RefundCreateOptions
            {
                PaymentIntent = transactionId,
                Amount = (long)(amount * 100) // Convert to cents
            };
            
            var refund = await service.CreateAsync(options);
            
            return new GatewayRefundResponse(
                refund.Status == "succeeded",
                refund.Id,
                refund.Status,
                amount,
                null);
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, $"Stripe refund failed for {transactionId}");
            return new GatewayRefundResponse(
                false, 
                null, 
                "failed", 
                0, 
                ex.Message);
        }
    }
}
