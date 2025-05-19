namespace Business.Services.ChechoutService;

using Business.ViewModels.CheckoutViewModels;

public interface ICheckoutService
{
    Task<CheckoutResult> ProcessCheckoutAsync(CheckoutViewModel model);
}
