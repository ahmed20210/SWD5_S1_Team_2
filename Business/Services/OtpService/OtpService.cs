using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Business.Services.MailingService;
namespace Business.Services.OtpService
{
    public class OtpService : IOtpService
    {
        private readonly IMailingService _mailingService;
        private readonly Dictionary<string, (string Code, DateTime Expiry)> _otpStore = new();

        public OtpService(IMailingService mailingService)
        {
            _mailingService = mailingService;
        }

        public string GenerateAndSendOtp(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            _otpStore[email] = (otp, DateTime.UtcNow.AddMinutes(5));

            _mailingService.SendMailAsync(email, "Verification Code", $"Your Code is: {otp}");
            return otp;
        }

        public bool VerifyOtp(string email, int otp)
        {
            // if (_otpStore.TryGetValue(email, out var entry))
            // {
            //     return entry.Code == otp && entry.Expiry > DateTime.UtcNow;
            // }
            return false;
        }
    }
}
