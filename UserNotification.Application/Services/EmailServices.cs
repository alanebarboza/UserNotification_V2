using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using UserNotification.Domain.Interfaces.Services;
using UserNotification.Shared.Entities;

namespace UserNotification.Application.Services
{
    public sealed class EmailServices : IEmailServices
    {
        private readonly EmailSettings _mailSettings;
        public EmailServices(IOptions<EmailSettings> emailSettings)
        {
            _mailSettings = emailSettings.Value;
        }
        public async Task SendEmail(Email email)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
            message.To.Add(new MailAddress(email.ToEmail));
            message.Subject = email.Subject;

            message.IsBodyHtml = false;
            message.Body = email.Body;
            smtp.Port = _mailSettings.Port;
            smtp.Host = _mailSettings.Host;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(message);
        }
    }
}
