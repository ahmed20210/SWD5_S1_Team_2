using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.OtpService
{
    public interface IOtpService
    {
        string GenerateAndSendOtp(string email);
        bool VerifyOtp(string email, int otp);
    }
}
