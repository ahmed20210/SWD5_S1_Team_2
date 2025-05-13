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
using Business.ViewModels.PaymentViewModels;
using Business.Services.PaymentService;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPaymentService _paymentService;
        public OrderService(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }



        public async Task<bool> CreateOrderAsync(CreateOrderViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validate Products
                var productIds = model.Items.Select(i => i.ProductId).ToList();
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

                if (products.Count != model.Items.Count)
                    return false;

                decimal total = 0;
                var orderItems = new List<OrderItem>();

                foreach (var item in model.Items)
                {
                    if (item.Quantity <= 0) return false;

                    var product = products.First(p => p.Id == item.ProductId);
                    var unitPrice = product.Price;

                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice,
                        TotalAmount = unitPrice * item.Quantity
                    };

                    total += orderItem.TotalAmount;
                    orderItems.Add(orderItem);
                }

                // Create Order
                var order = new Order
                {
                    CustomerId = model.CustomerId,
                    Date = DateTime.UtcNow,
                    TotalAmount = total,
                    OrderItems = orderItems
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Create Payment
                var payment = new CreatePaymentViewModel
                {
                    OrderId = order.Id,
                    Amount = total,
                    Method = model.PaymentMethod,
                    Status = PaymentStatus.Pending
                };

                await _paymentService.CreateAsync(payment);

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
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
