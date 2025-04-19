using Business.Services.OtpService;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class OtpController : Controller
    {
        private readonly IOtpService _otpService;

        public OtpController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost]
        public IActionResult SendOtp(string email)
        {
            _otpService.GenerateAndSendOtp(email);
            return View("EnterOtp", new VerifyOtpViewModel { Email = email });
        }

        [HttpPost]
        public IActionResult VerifyOtp(VerifyOtpViewModel model)
        {
            if (_otpService.VerifyOtp(model.Email, model.OtpCode))
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError("", "Invalid or expired OTP");
            return View("EnterOtp", model);
        }
    }
}
