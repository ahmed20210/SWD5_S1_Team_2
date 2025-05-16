using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _stripeSecretKey;
        private readonly string _stripePublishableKey;
        private readonly ILogger<StripeApiController> _logger;

        public StripeApiController(IConfiguration configuration, ILogger<StripeApiController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _stripeSecretKey = _configuration["Stripe:SecretKey"] ?? 
                throw new InvalidOperationException("Stripe:SecretKey is not configured");
            _stripePublishableKey = _configuration["Stripe:PublishableKey"] ?? 
                throw new InvalidOperationException("Stripe:PublishableKey is not configured");
            
            // Initialize Stripe configuration once at startup
            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        [HttpGet("GetPublicKey")]
        public IActionResult GetPublicKey()
        {
            return Ok(new { publicKey = _stripePublishableKey });
        }

        [HttpPost("CreatePaymentIntent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentCreateRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PaymentMethodId))
                {
                    return BadRequest(new { success = false, error = "Payment method ID is required" });
                }

                if (request.Amount <= 0)
                {
                    return BadRequest(new { success = false, error = "Amount must be greater than zero" });
                }

                // Create payment intent options
                var options = new PaymentIntentCreateOptions
                {
                    Amount = request.Amount,
                    Currency = request.Currency ?? "usd", // Default to USD if not specified
                    PaymentMethod = request.PaymentMethodId,
                    ConfirmationMethod = "manual",
                    Confirm = true,
                    // Add metadata for order tracking
                    Metadata = new Dictionary<string, string>
                    {
                        { "OrderId", Guid.NewGuid().ToString() },
                        { "CustomerEmail", request.CustomerEmail ?? "guest" }
                    }
                };

                // Create payment intent service
                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                // Handle the response based on PaymentIntent status
                if (paymentIntent.Status == "succeeded")
                {
                    // Payment was successful
                    var orderId = paymentIntent.Metadata["OrderId"];
                    _logger.LogInformation("Payment succeeded. OrderId: {OrderId}", orderId);
                    return Ok(new { success = true, orderId = orderId });
                }
                else if (paymentIntent.Status == "requires_action" || paymentIntent.Status == "requires_confirmation")
                {
                    // Additional authentication required
                    _logger.LogInformation("Payment requires additional action");
                    return Ok(new
                    {
                        success = false,
                        requiresAction = true,
                        clientSecret = paymentIntent.ClientSecret,
                        paymentIntentId = paymentIntent.Id
                    });
                }
                else
                {
                    // Payment failed
                    _logger.LogWarning("Payment failed with status: {Status}", paymentIntent.Status);
                    return Ok(new { success = false, error = $"Payment status: {paymentIntent.Status}" });
                }
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Stripe error: {StripeError}", e.Message);
                return BadRequest(new { success = false, error = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating payment intent");
                return StatusCode(500, new { success = false, error = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpPost("ConfirmPayment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentConfirmationRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PaymentIntentId))
                {
                    return BadRequest(new { success = false, error = "Payment intent ID is required" });
                }

                // Retrieve the payment intent
                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(request.PaymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    // Payment was successful
                    var orderId = paymentIntent.Metadata.ContainsKey("OrderId") 
                        ? paymentIntent.Metadata["OrderId"] 
                        : Guid.NewGuid().ToString();
                    
                    _logger.LogInformation("Payment confirmed. OrderId: {OrderId}", orderId);
                    return Ok(new { success = true, orderId = orderId });
                }
                else if (paymentIntent.Status == "requires_confirmation")
                {
                    // Confirm the payment if needed
                    paymentIntent = await service.ConfirmAsync(request.PaymentIntentId);
                    
                    if (paymentIntent.Status == "succeeded")
                    {
                        var orderId = paymentIntent.Metadata.ContainsKey("OrderId") 
                            ? paymentIntent.Metadata["OrderId"] 
                            : Guid.NewGuid().ToString();
                        
                        _logger.LogInformation("Payment confirmed after additional confirmation. OrderId: {OrderId}", orderId);
                        return Ok(new { success = true, orderId = orderId });
                    }
                    else
                    {
                        _logger.LogWarning("Payment confirmation failed with status: {Status}", paymentIntent.Status);
                        return Ok(new { success = false, error = $"Payment status after confirmation: {paymentIntent.Status}" });
                    }
                }
                else
                {
                    // Payment failed or is still pending
                    _logger.LogWarning("Payment confirmation failed with status: {Status}", paymentIntent.Status);
                    return Ok(new { success = false, error = $"Payment not completed. Status: {paymentIntent.Status}" });
                }
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Stripe error during payment confirmation: {StripeError}", e.Message);
                return BadRequest(new { success = false, error = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error confirming payment");
                return StatusCode(500, new { success = false, error = "An unexpected error occurred while confirming payment." });
            }
        }
    }

    public class PaymentIntentCreateRequest
    {
        public string PaymentMethodId { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string Currency { get; set; } = "usd";
        public string? CustomerEmail { get; set; }
    }

    public class PaymentConfirmationRequest
    {
        public string PaymentIntentId { get; set; } = string.Empty;
    }
}
