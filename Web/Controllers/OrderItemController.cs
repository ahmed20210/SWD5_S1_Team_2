using Microsoft.AspNetCore.Mvc;
using Business.Services.OrderItemService;
using Business.ViewModels.OrderItemViewModel;
using Domain.Entities;

namespace Web.Controllers
{
    
    public class OrderItemController : Controller
    {
        private readonly IOrderItemServiceWithUnitOfWork _orderItemService;

        public OrderItemController(IOrderItemServiceWithUnitOfWork orderItemService)
        {
            _orderItemService = orderItemService;
        }

        
        public async Task<IActionResult> Index()
        {
            // var orderItems = await _orderItemService.GetOrderItemsByOrderIdAsync();
            return View();
        }

        
        public async Task<IActionResult> Details(int orderId, int productId)
        {
            var orderItem = await _orderItemService.GetByIdAsync(orderId, productId);
            if (orderItem == null)
            {
                return NotFound();
            }

            var viewModel = new OrderItemViewModel
            {
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                ProductName = orderItem.Product.Name, 
                Quantity = orderItem.Quantity,
                TotalAmount = orderItem.TotalAmount,
                DiscountId = orderItem.DiscountId,
                DiscountAmount = orderItem.Discount?.Amount ?? 0 
            };

            return View(viewModel);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var orderItem = new OrderItem
                {
                    OrderId = viewModel.OrderId,
                    ProductId = viewModel.ProductId,
                    Quantity = viewModel.Quantity,
                    TotalAmount = viewModel.TotalAmount,
                    DiscountId = viewModel.DiscountId
                };

                await _orderItemService.CreateAsync(orderItem);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        
        public async Task<IActionResult> Edit(int orderId, int productId)
        {
            var orderItem = await _orderItemService.GetByIdAsync(orderId, productId);
            if (orderItem == null)
            {
                return NotFound();
            }

            var viewModel = new OrderItemViewModel
            {
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                ProductName = orderItem.Product.Name, 
                Quantity = orderItem.Quantity,
                TotalAmount = orderItem.TotalAmount,
                DiscountId = orderItem.DiscountId,
                DiscountAmount = orderItem.Discount?.Amount ?? 0
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var orderItem = new OrderItem
                {
                    OrderId = viewModel.OrderId,
                    ProductId = viewModel.ProductId,
                    Quantity = viewModel.Quantity,
                    TotalAmount = viewModel.TotalAmount,
                    DiscountId = viewModel.DiscountId
                };

                await _orderItemService.UpdateAsync(orderItem);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

       
        public async Task<IActionResult> Delete(int orderId, int productId)
        {
            var orderItem = await _orderItemService.GetByIdAsync(orderId, productId);
            if (orderItem == null)
            {
                return NotFound();
            }

            await _orderItemService.DeleteAsync(orderId, productId);
            return RedirectToAction(nameof(Index));
        }

        

    }


}
