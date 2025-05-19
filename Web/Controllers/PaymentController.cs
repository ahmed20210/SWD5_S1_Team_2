using Microsoft.AspNetCore.Mvc;


namespace Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _configuration;

        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult StripeCheckout(decimal amount = 160.00M)
        {
            ViewBag.Amount = amount;
            return View();
        }

        public IActionResult Success(string orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        public IActionResult Failure(string errorMessage = null)
        {
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }
    }
}
