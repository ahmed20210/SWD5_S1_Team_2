using Business.Services.OrderService;
using Business.ViewModels.CreateOrderViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
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
            if (!ModelState.IsValid)
                return View(model);

            await _orderService.CreateAsync(model);
            return RedirectToAction("Success"); 
        }
    }

}
