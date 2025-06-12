using HopeBox.Core.Config;
using MailKit.Net.Smtp;
using MimeKit;

namespace HopeBox.Core.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfig _config;

        public EmailService(IConfig config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailMessage = CreateEmailMessage(to, subject, body);
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(string to, string subject, string body)
        {
            var emailMessage = new MimeMessage();

            var emailConfig = _config.GetEmailConfiguration();

            emailMessage.From.Add(new MailboxAddress("HopeBox", emailConfig.From));
            emailMessage.To.Add(MailboxAddress.Parse(to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage emailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                var emailConfig = _config.GetEmailConfiguration();

                await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, true);
                await client.AuthenticateAsync(emailConfig.Username, emailConfig.Password);
                await client.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
