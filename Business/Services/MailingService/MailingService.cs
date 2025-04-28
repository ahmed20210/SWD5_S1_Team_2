using Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Business.Services.MailingService;

public class MailingService(IOptions<EmailSettings> mailSettings) : IMailingService
{
    private readonly EmailSettings _mailSettings = mailSettings.Value;

    public async Task SendMailAsync(string to, string subject, string body, IList<IFormFile>? attachments = null)
    {
        var email = new MimeMessage
        {
            Subject = subject
        };
        Console.WriteLine($"Sender Email: {_mailSettings}");
        email.Sender = MailboxAddress.Parse(_mailSettings.SenderEmail);
        email.To.Add(MailboxAddress.Parse(to));

        var bodyBuilder = new BodyBuilder { HtmlBody = body };

        if (attachments is not null)
        {
            foreach (var file in attachments)
            {
                if (file.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    bodyBuilder.Attachments.Add(file.FileName, ms.ToArray(), ContentType.Parse(file.ContentType));
                }
            }
        }

        email.Body = bodyBuilder.ToMessageBody();
        email.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_mailSettings.SenderEmail, _mailSettings.SenderPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
