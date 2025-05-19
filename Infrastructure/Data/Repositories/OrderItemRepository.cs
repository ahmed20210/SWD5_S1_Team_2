using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public interface IOrderItemRepository : IRepository<OrderItem>
{
    Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
    Task<IEnumerable<OrderItem>> GetOrderItemsByProductIdAsync(int productId);
}

public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
    {
        return await _context.Set<OrderItem>()
            .Where(oi => oi.OrderId == orderId)
            .Include(oi => oi.Product)
            .Include(oi => oi.Discount)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsByProductIdAsync(int productId)
    {
        return await _context.Set<OrderItem>()
            .Where(oi => oi.ProductId == productId)
            .Include(oi => oi.Product)
            .ToListAsync();
    }
}
