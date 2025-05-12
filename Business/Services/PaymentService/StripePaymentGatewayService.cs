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
            return new GatewayVerificationResponse(false, 0, null, "failed", DateTime.MinValue);
        }
    }
    
    public async Task<GatewayRefundResponse> RefundPaymentAsync(string transactionId, decimal amount)
    {
        try
        {
            var options = new RefundCreateOptions
            {
                PaymentIntent = transactionId,
                Amount = (long)(amount * 100), // Convert to cents
                Reason = RefundReasons.RequestedByCustomer
            };

            var service = new RefundService(_client);
            var refund = await service.CreateAsync(options);

            return new GatewayRefundResponse(
                Success: true,
                RefundId: refund.Id,
                Status: refund.Status,
                AmountRefunded: refund.Amount / 100m
            );
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, $"Stripe refund failed for {transactionId}");
            return new GatewayRefundResponse(
                RefundId: null,
                Status: null,
                Success: false,
                ErrorMessage: ex.StripeError?.Message ?? ex.Message 
            );
        }
    }
}