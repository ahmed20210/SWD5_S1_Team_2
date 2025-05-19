using Business.Services.OrderService;
using Business.ViewModels.CreateOrderViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  
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

    }

}
