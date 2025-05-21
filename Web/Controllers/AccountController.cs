using Business.Services.AccountService;
using Business.Services.AddressService;
using Domain;
using Domain.Entities;
using Business.ViewModels.UserViewModel;
using Business.ViewModels.AddressViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;

        public AccountController(IAccountService accountService, IAddressService addressService)
        {
            _accountService = accountService;
            _addressService = addressService;
        }

        // Add the Orders action to redirect to OrderController's UserOrders action
        public IActionResult Orders()
        {
            return RedirectToAction("UserOrders", "Order");
        }

        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _accountService.SignUpAsync(model, UserRole.Client);

                    if (result.Success)
                    {
                        return RedirectToAction(nameof(Verify), new { email = model.Email });
                    }

                    ModelState.AddModelError(string.Empty, result.Message);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error during SignUp: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult LogIn(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _accountService.LogInAsync(model);

                    if (result.Success)
                    {
                        return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                            ? Redirect(returnUrl)
                            : RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, result.Message);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error during LogIn: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _accountService.LogOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Verify(string email)
        {
            var model = new VerifyOtpViewModel
            {
                Email = email
            };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Verify(VerifyOtpViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _accountService.VerifyUserAsync(model.Email, model.OtpCode);
                    if (result.Success)
                    {
                        return RedirectToAction(nameof(LogIn));
                    }

                    ModelState.AddModelError(string.Empty, result.Message);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error during Verify: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                }
            }

            return View(model);
        }

[Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> Settings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn");
            }
            
            var user = await _accountService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("LogIn");
            }

            var viewModel = new UserProfileViewModel
            {
                Id = user.Id,
                FName = user.FName,
                LName = user.LName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            };

            ViewBag.ChangePasswordModel = new ChangePasswordViewModel();
            
            // Get addresses for the user
            var addressesResponse = await _addressService.GetAllUserAddressesAsync(userId);
            if (addressesResponse.Success)
            {
                ViewBag.Addresses = addressesResponse.Data;
            }
            
            return View(viewModel);
        }


        [Authorize(Roles = "Client,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ChangePasswordModel = new ChangePasswordViewModel();
                ViewBag.ProfileTab = true;
                return View("Settings", model);
            }

            var result = await _accountService.UpdateUserProfileAsync(model);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Profile updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction("Settings");
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SecurityTab = true;
                return View("Settings", new UserProfileViewModel());
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn");
            }

            var result = await _accountService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Password changed successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                ViewBag.SecurityTab = true;
            }

            return RedirectToAction("Settings");
        }
        [Authorize(Roles = "Client,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(CreateAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please check your input and try again";
                return RedirectToAction("Settings");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn");
            }

            var result = await _addressService.CreateAddressAsync(model, userId);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Address added successfully";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
            
            return RedirectToAction("Settings");
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(int id, UpdateAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please check your input and try again";
                return RedirectToAction("Settings");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn");
            }

            var result = await _addressService.UpdateAddressAsync(model, id, userId);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Address updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
            
            return RedirectToAction("Settings");
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn");
            }

            var result = await _addressService.DeleteAddressAsync(id, userId);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Address deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
            
            return RedirectToAction("Settings");
        }
        [Authorize(Roles = "Client,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetMainAddress(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("LogIn");
            }

            var result = await _addressService.SetAsMainAddressAsync(id, userId);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Main address updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
            
            return RedirectToAction("Settings");
        }
    }
}

