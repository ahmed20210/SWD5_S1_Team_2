using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public interface IOrderTimeLineRepository : IRepository<OrderTimeLine>
{
    Task<IEnumerable<OrderTimeLine>> GetTimeLineByOrderIdAsync(int orderId);
    Task<OrderTimeLine> GetLatestStatusForOrderAsync(int orderId);
}

public class OrderTimeLineRepository : Repository<OrderTimeLine>, IOrderTimeLineRepository
{
    public OrderTimeLineRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<OrderTimeLine>> GetTimeLineByOrderIdAsync(int orderId)
    {
        return await _context.OrderTimeLine
            .Where(otl => otl.OrderId == orderId)
            .OrderByDescending(otl => otl.ChangedAt)
            .ToListAsync();
    }

    public async Task<OrderTimeLine> GetLatestStatusForOrderAsync(int orderId)
    {
        return await _context.OrderTimeLine
            .Where(otl => otl.OrderId == orderId)
            .OrderByDescending(otl => otl.ChangedAt)
            .FirstOrDefaultAsync();
    }
}
