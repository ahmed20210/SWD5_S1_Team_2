using Business.Services.AccountService;
using Domain;
using Domain.Entities;
using Domain.ViewModels.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
        [HttpGet]
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
        public async Task<IActionResult> Verify(string email, int OtpCode)
        {
            if (ModelState.IsValid)
            {
                try
                {
                  
                    var result = await _accountService.VerifyUserAsync(email, OtpCode);

                    if (result.Success)
                    {
                        return RedirectToAction("Index", "Home");
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

            var model = new VerifyOtpViewModel
            {
                Email = email,
                OtpCode = OtpCode
            };
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _accountService.LogInAsync(model);

                    if (result.Success)
                    {
                        return RedirectToAction("Index", "Home");
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

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                var result = await _accountService.LogOutAsync();
                if (result.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, result.Message);
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

