using Business.ViewModels.CreateOrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.OrderService
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(CreateOrderViewModel model);
        Task<OrderCompletionResult> CompleteOrderAsync(int orderId);
    }

}
public record OrderCompletionResult(bool Success, string? Message = null)
{
    // Factory methods
    public static OrderCompletionResult SuccessResult() 
        => new(true);
    
    public static OrderCompletionResult Failed(string message) 
        => new(false, message);
}
