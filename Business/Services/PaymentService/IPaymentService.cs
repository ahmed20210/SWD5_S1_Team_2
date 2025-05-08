using Business.ViewModels.PaymentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentViewModel>> GetAllAsync();
        Task CreateAsync(CreatePaymentViewModel model);
        Task<PaymentViewModel> GetByIdAsync(int id);
        Task<PaymentViewModel> EditAsync(int id, UpdateViewModel model);
        Task<UpdateViewModel> GetByIdForUpdateAsync(int id);


        Task<bool> DeleteAsync(int id);
    }
}