using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> GetOrderWithDetailsAsync(int id);
    Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
}

public class OrderRepository : Repository<Order>, IOrderRepository
{
    
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Order> GetOrderWithDetailsAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderTimeLines)
            .Include(o => o.Payment)
            .Include(o => o.Address)
            .Include(o => o.Coupon)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.OrderItems)
            .Include(o => o.Payment)
            .OrderByDescending(o => o.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        return await _context.Orders
            .Where(o => o.Status == status)
            .Include(o => o.Customer)
            .Include(o => o.Payment)
            .OrderByDescending(o => o.Date)
            .ToListAsync();
    }
}
