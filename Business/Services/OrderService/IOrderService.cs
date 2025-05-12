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
        Task CreateAsync(CreateOrderViewModel model);
    }

}
