using Business.Services.AccountService;
using Business.ViewModels.UserViewModel;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("QuickSignup")]
        public async Task<IActionResult> QuickSignup([FromBody] SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid input data", errors = ModelState });
            }

            try
            {
                var result = await _accountService.SignUpAsync(model, UserRole.Client);

                if (result.Success)
                {
                    // We don't want to automatically log in the user since they need to verify their email
                    return Ok(new { success = true, message = "Account created successfully", email = model.Email });
                }

                return BadRequest(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("QuickLogin")]
        public async Task<IActionResult> QuickLogin([FromBody] LogInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid login data", errors = ModelState });
            }

            try
            {
                var result = await _accountService.LogInAsync(model);

                if (result.Success)
                {
                    return Ok(new { success = true, message = "Login successful" });
                }

                return BadRequest(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp(string email, int otpCode)
        {
            if (string.IsNullOrEmpty(email) || otpCode <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid verification data" });
            }

            try
            {
                var result = await _accountService.VerifyUserAsync(email, otpCode);

                if (result.Success)
                {
                    return Ok(new { success = true, message = "Verification successful" });
                }

                return BadRequest(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("ResendOtp")]
        public async Task<IActionResult> ResendOtp(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { success = false, message = "Email is required" });
            }

            try
            {
                var result = await _accountService.ResendOtpAsync(email);

                if (result.Success)
                {
                    return Ok(new { success = true, message = "Verification code has been resent" });
                }

                return BadRequest(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
