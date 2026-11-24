using EduPro.Data.Helpers;
using EduPro.Service.Abstracts;
using MailKit.Net.Smtp;
using MimeKit;

namespace EduPro.Service.Implementations
{
    public class EmailsService : IEmailsService
    {
        private readonly EmailSettings _emailSettings;

        public EmailsService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }
        public async Task<string> SendEmail(string email, string message, string? reason)
        {
            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, true);
                client.Authenticate(_emailSettings.FromEmail, _emailSettings.Password);

                var bodyBuilder = new BodyBuilder { HtmlBody = message };
                var mimeMessage = new MimeMessage { Body = bodyBuilder.ToMessageBody() };
                mimeMessage.From.Add(new MailboxAddress("EduPro", _emailSettings.FromEmail));
                mimeMessage.To.Add(new MailboxAddress("", email));
                mimeMessage.Subject = reason ?? "EduPro Notification";

                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
                return "Success";
            }
            catch
            {
                return "Failed";
            }
        }
    }
}
