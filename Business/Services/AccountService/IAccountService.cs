using Domain;
using Domain.Response;
using Business.ViewModels.UserViewModel;
using Domain.Entities;

namespace Business.Services.AccountService
{
    public interface IAccountService
    {
        Task<BaseResponse> SignUpAsync(SignUpViewModel userModel, UserRole role = UserRole.Client);
        Task<BaseResponse> VerifyUserAsync(string email, int verificationCode);
        Task<BaseResponse> LogInAsync(LogInViewModel userModel);
        Task<BaseResponse> LogOutAsync();
        
        // New methods for account settings
        Task<User> GetUserByIdAsync(string userId);
        Task<BaseResponse> UpdateUserProfileAsync(UserProfileViewModel model);
        Task<BaseResponse> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    }
}

