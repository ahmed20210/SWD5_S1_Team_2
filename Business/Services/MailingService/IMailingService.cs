namespace Business.Services.MailingService;
using Microsoft.AspNetCore.Http;

public interface IMailingService
{
    Task SendMailAsync(string to, string subject, string body , IList<IFormFile>? attachments = null);
}