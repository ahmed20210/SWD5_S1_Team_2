using Domain;
using Domain.Response;
using Business.ViewModels.UserViewModel;

namespace Business.Services.AccountService
{
    public interface IAccountService
    {
        Task<BaseResponse> SignUpAsync(SignUpViewModel userModel, UserRole role = UserRole.Client);
        Task<BaseResponse> VerifyUserAsync(string email, int verificationCode);
        Task<BaseResponse> LogInAsync(LogInViewModel userModel);

        Task<BaseResponse> LogOutAsync();
    }
}

