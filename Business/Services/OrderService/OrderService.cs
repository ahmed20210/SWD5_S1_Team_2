using Business.ViewModels.CreateOrderViewModels;
using Domain.Entities;
using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Business.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateOrderViewModel model)
        {
            var order = new Order
            {
                CustomerId = model.CustomerId,
                TotalAmount = model.TotalAmount,
                Date = DateTime.UtcNow,
                OrderItems = model.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    TotalAmount = i.UnitPrice
                }).ToList(),

                Payment = new Payment
                {
                    Amount = model.TotalAmount,
                    Method = model.PaymentMethod,
                    Status = PaymentStatus.Success,
                    TransactionId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                }
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
        public async Task<OrderCompletionResult> CompleteOrderAsync(int orderId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
        
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)  
                    .FirstOrDefaultAsync(o => o.Id == orderId);
            
                if (order == null)
                    return OrderCompletionResult.Failed("Order not found");
            
                if (order.Status == OrderStatus.Shipped)
                    return OrderCompletionResult.Failed("Order already completed");
            
                // 1. Update order status
                order.Status = OrderStatus.Shipped;
                order.Date = DateTime.UtcNow;
            
                // 2. Update inventory
                foreach (var item in order.OrderItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    product.Stock -= item.Quantity;
                }
            
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            
                // 3. Send notifications
                return OrderCompletionResult.SuccessResult();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"Order completion failed for {orderId}");
                return OrderCompletionResult.Failed("Order completion failed");
            }
        }
    }
    
}
