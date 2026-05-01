using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Dto;
using NotificationService.Interfaces;

namespace NotificationService.Services
{
    public class EmailSender(
        IOptions<EmailOptions> emailOptions,
        ILogger<EmailSender> logger) : IEmailSender
    {
        private EmailOptions EmailOptionsValue => emailOptions.Value;

        public async Task SendEmailAsync(NotificationEvent notificationEvent)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("InternshipPlatform", EmailOptionsValue.Email));
            emailMessage.To.Add(new MailboxAddress("", notificationEvent.Email));
            emailMessage.Subject = notificationEvent.Title;
            emailMessage.Body = new TextPart("plain") { Text = notificationEvent.Message };

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(
                    EmailOptionsValue.Host,
                    EmailOptionsValue.Port,
                    SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(EmailOptionsValue.Email, EmailOptionsValue.Password);

                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }
}
