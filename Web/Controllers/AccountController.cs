using Business.Services.AccountService;
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
                var result = await _accountService.SignUpAsync(model);
                // log the result
                Console.WriteLine(result.Message);
            
                if (result.Success)
                {
                    return RedirectToAction(nameof(Verify), new { email = model.Email });
                }

                ModelState.AddModelError(string.Empty, result.Message);
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
        public async Task<IActionResult> Verify(string email, int verificationCode)
        {


            if (ModelState.IsValid)
            {
                var result = await _accountService.VerifyUserAsync(email, verificationCode);

                if (result.Success)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, result.Message);
            }
            var model = new VerifyOtpViewModel
            {
                Email = email,
                OtpCode = verificationCode
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
                var result = await _accountService.LogInAsync(model);

                if (result.Success)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, result.Message);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            var result = await _accountService.LogOutAsync();
            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return RedirectToAction("Index", "Home");
        }
    }
}

