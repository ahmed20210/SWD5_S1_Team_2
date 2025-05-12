using Business.ViewModels.CreateOrderViewModels;
using Domain.Entities;
using Domain;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

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
    }

}
