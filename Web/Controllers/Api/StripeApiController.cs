
using Business.Services.ChechoutService;
using Business.ViewModels.CheckoutViewModels;
using Business.ViewModels.PaymentViewModels;
using Business.ViewModels.CreateOrderViewModels;
using Business.ViewModels.AddressViewModels;
using Business.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Domain;
using System.Security.Claims;
using Infrastructure.Data;
using System.Text.Json;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeApiController : Controller
    {
        private readonly ICheckoutService _checkoutService;
        private readonly IConfiguration _configuration;
        private readonly string _stripeSecretKey;
        private readonly string _stripePublicKey;
        private readonly ILogger<StripeApiController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public StripeApiController(
            ICheckoutService checkoutService,
            IConfiguration configuration,
            ILogger<StripeApiController> logger,
            ApplicationDbContext dbContext)
        {
            _checkoutService = checkoutService;
            _configuration = configuration;
            _stripeSecretKey = _configuration["Stripe:SecretKey"];
            _stripePublicKey = _configuration["Stripe:PublishableKey"];
            StripeConfiguration.ApiKey = _stripeSecretKey;
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("GetPublicKey")]
        public IActionResult GetPublicKey()
        {
            return Ok(new { publicKey = _stripePublicKey });
        }

        [HttpPost("CreatePaymentIntent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = request.Amount,
                    Currency = "usd",
                    PaymentMethod = request.PaymentMethodId,
                    Confirm = true,
                    ReturnUrl = $"{Request.Scheme}://{Request.Host}/payment/success",
                    Customer = request.CustomerEmail != null ? await GetOrCreateCustomer(request.CustomerEmail) : null,
                    Metadata = new System.Collections.Generic.Dictionary<string, string>
                    {
                        { "OrderId", request.OrderId.ToString() }
                    }
                };

                var service = new PaymentIntentService();
                var intent = await service.CreateAsync(options);

                return Ok(new
                {
                    success = true,
                    clientSecret = intent.ClientSecret,
                    paymentIntentId = intent.Id,
                    requiresAction = intent.Status == "requires_action" || intent.Status == "requires_source_action"
                });
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe error creating payment intent for OrderId: {OrderId}, Amount: {Amount}", 
                    request.OrderId, request.Amount);
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("ConfirmPayment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
        {
            try
            {
                var service = new PaymentIntentService();
                
                // First check the current status of the payment intent
                var intent = await service.GetAsync(request.PaymentIntentId);
                
                // Only attempt to confirm if it's not already succeeded
                if (intent.Status != "succeeded")
                {
                    intent = await service.ConfirmAsync(request.PaymentIntentId);
                }

                if (intent.Status == "succeeded")
                {
                    // Create checkout view model
                    var checkoutViewModel = new CheckoutViewModel
                    {
                        Order = request.Order,
                        Payment = new CreatePaymentViewModel
                        {
                            TransactionId = intent.Id, 
                            Amount = intent.Amount / 100m, // Convert from cents to dollars
                            Status = PaymentStatus.Success,
                            Method = PaymentMethods.Stripe
                        },
                        ShippingAddress = request.ShippingAddress
                    };
                    checkoutViewModel.Order.CustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    // Process the checkout
                    var result = await _checkoutService.ProcessCheckoutAsync(checkoutViewModel);

                    if (result.IsSuccess)
                    {
                        return Ok(new { success = true, orderId = result.OrderId });
                    }
                    else
                    {
                        return BadRequest(new { success = false, error = result.Message });
                    }
                }
                else
                {
                    return BadRequest(new { success = false, error = "Payment confirmation failed" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming payment for PaymentIntentId: {PaymentIntentId}", request.PaymentIntentId);
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("PrepareCheckout")]
        public async Task<IActionResult> PrepareCheckout([FromBody] PrepareCheckoutRequest request)
        {
            try
            {
                // Calculate total amount from cart items
                decimal totalAmount = 0;
                foreach (var item in request.Cart)
                {
                    // Get product price from database
                    var product = await _dbContext.Products.FindAsync(item.ProductId);
                    if (product == null)
                    {
                        return BadRequest(new { success = false, error = $"Product with ID {item.ProductId} not found" });
                    }
                    totalAmount += product.Price * item.Quantity;
                }

                // Create temporary order to get ID
                var order = new CreateOrderViewModel
                {
                    CustomerId = User.FindFirst("sub")?.Value,
                    TotalAmount = totalAmount,
                    Items = request.Cart.Select(item => new CreateOrderItemViewModel
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    }).ToList()
                };

                // Store order and shipping address in TempData for later use
                TempData["OrderData"] = JsonSerializer.Serialize(order);
                if (request.ShippingAddress != null)
                {
                    TempData["ShippingAddress"] = JsonSerializer.Serialize(request.ShippingAddress);
                }

                return Ok(new
                {
                    success = true,
                    orderId = 0, // This will be replaced with actual order ID after payment
                    amount = (int)(totalAmount * 100) // Convert to cents for Stripe
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing checkout");
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        private async Task<string> GetOrCreateCustomer(string email)
        {
            var customerService = new CustomerService();
            var customers = await customerService.ListAsync(new CustomerListOptions
            {
                Email = email,
                Limit = 1
            });

            if (customers.Data.Count > 0)
            {
                return customers.Data[0].Id;
            }

            var customer = await customerService.CreateAsync(new CustomerCreateOptions
            {
                Email = email
            });

            return customer.Id;
        }
    }

    public class CreatePaymentIntentRequest
    {
        public string PaymentMethodId { get; set; }
        public int Amount { get; set; }
        public string CustomerEmail { get; set; }
        public int OrderId { get; set; }
    }

    public class ConfirmPaymentRequest
    {
        public string PaymentIntentId { get; set; }
       
        public CreateOrderViewModel Order { get; set; }
        public CreateAddressViewModel? ShippingAddress { get; set; }
    }

    public class PrepareCheckoutRequest
    {
        public List<CartItem> Cart { get; set; }
        public CreateAddressViewModel ShippingAddress { get; set; }
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
