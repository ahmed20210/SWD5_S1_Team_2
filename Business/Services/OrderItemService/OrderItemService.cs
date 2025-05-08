using Business.ViewModels.OrderItemViewModel;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.OrderItemService
{

    public class OrderItemService : IOrderItemService
    {
        private readonly ApplicationDbContext _context;

        public OrderItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<OrderItem> CreateAsync(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<OrderItem> GetByIdAsync(int orderId, int productId)
        {
            return await _context.OrderItems.FirstOrDefaultAsync(oi => oi.OrderId == orderId && oi.ProductId == productId);
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _context.OrderItems.ToListAsync();
        }

        public async Task<OrderItem> UpdateAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<bool> DeleteAsync(int orderId, int productId)
        {
            var orderItem = await GetByIdAsync(orderId, productId);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<OrderItemViewModel>> GetOrderItemsViewModelAsync()
        {
            var orderItems = await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Discount)
                .ToListAsync();

            return orderItems.Select(oi => new OrderItemViewModel
            {
                OrderId = oi.OrderId,
                ProductId = oi.ProductId,
                ProductName = oi.Product.Name, 
                Quantity = oi.Quantity,
                TotalAmount = oi.TotalAmount,
                DiscountId = oi.DiscountId,
                DiscountAmount = oi.Discount?.Amount ?? 0 
            }).ToList();
        }
    }
}