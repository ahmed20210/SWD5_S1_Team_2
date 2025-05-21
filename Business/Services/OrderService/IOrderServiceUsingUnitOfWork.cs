using Business.ViewModels.CreateOrderViewModels;
using Domain.Entities;


namespace Business.Services.OrderService
{
    public interface IOrderServiceUsingUnitOfWork
    {
        Task<int> CreateOrderAsync(CreateOrderViewModel model);
        Task<OrderCompletionResult> CompleteOrderAsync(int orderId, int? paymentId);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId);
        Task<Order> GetOrderWithDetailsAsync(int orderId);
    }

    public record OrderCompletionResult(bool Success, string? Message = null)
    {
        // Factory methods
        public static OrderCompletionResult SuccessResult() 
            => new(true);
        
        public static OrderCompletionResult Failed(string message) 
            => new(false, message);
    }
}
