using Business.ViewModels.CreateOrderViewModels;
using Business.ViewModels.PaymentViewModels;
using Business.Services.PaymentService;
using Domain.Entities;
using Domain;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


namespace Business.Services.OrderService
{
    public class OrderServiceUsingUnitOfWork : IOrderServiceUsingUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderServiceUsingUnitOfWork> _logger;


        public OrderServiceUsingUnitOfWork(
            ApplicationDbContext context,
            IUnitOfWork unitOfWork,
            ILogger<OrderServiceUsingUnitOfWork> logger)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _logger = logger;

        }

        public async Task<int> CreateOrderAsync(CreateOrderViewModel model)
        {
            try
            {
                var order = new Order
                {
                    CustomerId = model.CustomerId,
                    Date = DateTime.UtcNow,
                    TotalAmount = model.TotalAmount,
                    Status = OrderStatus.Pending,
                    AddressId = model.AddressId,
                };


                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.CompleteAsync();

                return order.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return -1;
            }
        }

        public async Task<OrderCompletionResult> CompleteOrderAsync(int orderId, int? paymentId)
        {
            // Validate orderId and paymentId
            if (orderId <= 0)
                return OrderCompletionResult.Failed("Invalid order ID");

            if (paymentId <= 0)
                return OrderCompletionResult.Failed("Invalid payment ID");
        
            try
            {
                var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(orderId);

                if (order == null)
                    return OrderCompletionResult.Failed("Order not found");

                if (order.Status == OrderStatus.Shipped)
                    return OrderCompletionResult.Failed("Order already completed");

                order.Status = OrderStatus.Shipped;
                order.PaymentId = paymentId;
                await _unitOfWork.CompleteAsync();

                return OrderCompletionResult.SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Order completion failed for order {OrderId}", orderId);
                return OrderCompletionResult.Failed("Order completion failed");
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId)
        {
            try
            {
                return await _unitOfWork.Orders.GetOrdersByCustomerIdAsync(customerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders for customer {CustomerId}", customerId);
                return new List<Order>();
            }
        }

        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            try
            {
                return await _unitOfWork.Orders.GetOrderWithDetailsAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order details for order {OrderId}", orderId);
                return null;
            }
        }
    }
}
