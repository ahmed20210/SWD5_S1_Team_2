using AutoMapper;
using Business.Services.MailingService;
using Domain;
using Domain.Entities;
using Domain.Response;
using Domain.ViewModels.UserViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Business.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;

        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IMailingService _mailingService;

    

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IMailingService mailingService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailingService = mailingService;
        }

        public async Task<BaseResponse> SignUpAsync(SignUpViewModel userModel, UserRole role)
        {
            var user = await _userManager.FindByEmailAsync(userModel.Email);

            if (user != null)
            {
                return BaseResponse.FailureResponse("User already exists");
            }

            var phoneUser = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == userModel.PhoneNumber);
            if (phoneUser != null)
            {
                return BaseResponse.FailureResponse("Phone number already exists");
            }

            
            int VerificationCode = new Random().Next(100000, 999999);
            user = new User()
            {
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
                IsAgree = userModel.IsAgree,
                FName = userModel.FName,
                LName = userModel.LName,
                IsVerified = false,
                VerificationCode = VerificationCode,
                Role= role,
            };

            await _mailingService.SendMailAsync(
                to: userModel.Email,
                subject: "Verification Code",
                body: $"Your verification code is {VerificationCode}"
            );

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role.ToString());

                return BaseResponse.SuccessResponse("User created successfully");
            }


            return BaseResponse.FailureResponse("User creation failed", result.Errors.Select(e => e.Description).ToList());
            
            
        }

        public async Task<BaseResponse> VerifyUserAsync(string email, int verificationCode)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BaseResponse.FailureResponse("User not found");
            }

            if (user.VerificationCode != verificationCode)
            {
                return BaseResponse.FailureResponse("Invalid verification code");
            }

            user.IsVerified = true;
            await _userManager.UpdateAsync(user);

        
            await _signInManager.SignInAsync(user, isPersistent: true);

            return BaseResponse.SuccessResponse("User verified successfully");
        }


        public async Task<BaseResponse>LogInAsync(LogInViewModel userModel)
        {   
            var user = await _userManager.FindByEmailAsync(userModel.Email);

            if (user == null)
            {
                return BaseResponse.FailureResponse("User not found");
            }

            var result = await _signInManager.PasswordSignInAsync(user, userModel.Password, userModel.RememberMe, false);

            if (result.Succeeded)
            {
                return BaseResponse.SuccessResponse("Login successful");
            }

            return BaseResponse.FailureResponse("Invalid login attempt");
        }

        public async Task<BaseResponse> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return BaseResponse.SuccessResponse("Logout successful");
        }

        
    }
}
