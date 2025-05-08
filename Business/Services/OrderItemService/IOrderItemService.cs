using Business.ViewModels.OrderItemViewModel;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.OrderItemService
{


    public interface IOrderItemService
    {
        Task<OrderItem> CreateAsync(OrderItem orderItem);
        Task<OrderItem> GetByIdAsync(int orderId, int productId);
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<OrderItem> UpdateAsync(OrderItem orderItem);
        Task<bool> DeleteAsync(int orderId, int productId);
        Task<IEnumerable<OrderItemViewModel>> GetOrderItemsViewModelAsync();
    }



}
