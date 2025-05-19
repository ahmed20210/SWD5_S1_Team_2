using Business.ViewModels.OrderItemViewModel;
using Domain.Entities;

namespace Business.Services.OrderItemService;

public interface IOrderItemServiceWithUnitOfWork
{
    Task<OrderItem> CreateAsync(OrderItem orderItem);
    Task<bool> CreateRangeAsync(List<CreateRangeOrderItemsVM> orderItems, int orderId, decimal totalAmountConfirmed);
    Task<OrderItem> GetByIdAsync(int orderId, int productId);
    Task<IEnumerable<OrderItem>> GetAllAsync();
    Task<OrderItem> UpdateAsync(OrderItem orderItem);
    Task<bool> DeleteAsync(int orderId, int productId);
    Task<IEnumerable<OrderItemViewModel>> GetOrderItemsByOrderIdAsync(int orderId);
}
