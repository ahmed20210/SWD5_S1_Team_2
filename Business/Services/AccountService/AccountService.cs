using AutoMapper;
using Business.Services.MailingService;
using Domain;
using Domain.Entities;
using Domain.Response;
using Business.ViewModels.UserViewModel;
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
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IMailingService mailingService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailingService = mailingService;
            _roleManager = roleManager;
        }

        public async Task<BaseResponse> SignUpAsync(SignUpViewModel userModel, UserRole role)
        {

            if (!await _roleManager.RoleExistsAsync(role.ToString()))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(role.ToString()));
                if (!roleResult.Succeeded)
                {
                    return BaseResponse.FailureResponse("Failed to create role");
                }
            }

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
                UserName = userModel.Email,
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
                IsAgree = userModel.IsAgree,
                FName = userModel.FName,
                LName = userModel.LName,
                IsVerified = false,
                VerificationCode = VerificationCode,
                Role = role,
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

            return BaseResponse.FailureResponse("User creation failed", result.Errors);
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

        public async Task<BaseResponse> LogInAsync(LogInViewModel userModel)
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

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<BaseResponse> UpdateUserProfileAsync(UserProfileViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return BaseResponse.FailureResponse("User not found");
            }

            // Check if email is being changed and if it's already taken
            if (user.Email != model.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    return BaseResponse.FailureResponse("Email is already in use by another account");
                }

                user.Email = model.Email;
                user.UserName = model.Email; // Username is the same as email in our system
            }

            // Check if phone number is being changed and if it's already taken
            if (user.PhoneNumber != model.PhoneNumber)
            {
                var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber && u.Id != user.Id);
                if (existingUser != null)
                {
                    return BaseResponse.FailureResponse("Phone number is already in use by another account");
                }

                user.PhoneNumber = model.PhoneNumber;
            }

            // Update other properties
            user.FName = model.FName;
            user.LName = model.LName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return BaseResponse.SuccessResponse("Profile updated successfully");
            }

            return BaseResponse.FailureResponse("Failed to update profile", result.Errors);
        }

        public async Task<BaseResponse> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BaseResponse.FailureResponse("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (result.Succeeded)
            {
                // Sign the user in again with the new password
                await _signInManager.SignInAsync(user, isPersistent: false);
                return BaseResponse.SuccessResponse("Password changed successfully");
            }

            return BaseResponse.FailureResponse("Failed to change password", result.Errors);
        }
    }
}
