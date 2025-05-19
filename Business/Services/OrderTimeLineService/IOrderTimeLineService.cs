using Domain.Entities;
using Domain;
namespace Business.Services.OrderTimeLineService;

public interface IOrderTimeLineService
{
    Task<OrderTimeLine> AddTimeLineEntryAsync(int orderId, OrderStatus status, string description);
    Task<IEnumerable<OrderTimeLine>> GetTimeLineByOrderIdAsync(int orderId);
    Task<OrderTimeLine> GetLatestStatusForOrderAsync(int orderId);
}
