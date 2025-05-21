using Business.Services.OrderService;
using Business.ViewModels.CreateOrderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize(Roles = "Client,Admin")]

    public class OrderController : Controller
    {
        private readonly IOrderServiceUsingUnitOfWork _orderService;

        public OrderController(IOrderServiceUsingUnitOfWork orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _orderService.CreateOrderAsync(model);
            if (result > 0)
                return BadRequest("Not Valid");

            return RedirectToAction("Index");
        }

        // Get user orders
        public async Task<IActionResult> UserOrders()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetOrdersByCustomerIdAsync(userId);

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderWithDetailsAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpGet]
        public IActionResult NotAuthorized()
        {
            return View("NotAuthorized");
        }

        [HttpGet]
        public IActionResult NotFoundPage()
        {
            return View("NotFound");
        }
    }

}
