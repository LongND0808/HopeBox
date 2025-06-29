using HopeBox.Core.Config;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace HopeBox.Core.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfig _config;

        public EmailService(IConfig config)
        {
            _config = config;
        }

        public async Task<IEnumerable<EmailData>> GetEmailsFromSenderAsync(string sender)
        {
            var emailConfig = _config.GetEmailConfiguration();
            var emails = new List<EmailData>();

            using var client = new ImapClient();
            try
            {
                // Connect to Gmail IMAP server
                await client.ConnectAsync("imap.gmail.com", 993, true);

                // Authenticate using TrackingEmail and TrackingPassword
                await client.AuthenticateAsync(emailConfig.TrackingEmail, emailConfig.TrackingPassword);

                // Access the Inbox folder
                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                // Search for emails from the specified sender
                var query = SearchQuery.FromContains(sender).And(SearchQuery.DeliveredAfter(DateTime.Now.AddDays(-7)));
                var uids = await inbox.SearchAsync(query);

                // Fetch emails
                foreach (var uid in uids)
                {
                    var message = await inbox.GetMessageAsync(uid);
                    var emailData = new EmailData
                    {
                        Subject = message.Subject,
                        Body = message.TextBody ?? message.HtmlBody,
                        ReceivedDate = message.Date.UtcDateTime,
                        From = message.From.ToString()
                    };
                    emails.Add(emailData);
                }

                return emails;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy email: {ex.Message}", ex);
            }
            finally
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true);
                }
                client.Dispose();
            }
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
                throw new Exception($"Lỗi khi gửi email: {ex.Message}", ex);
            }
            finally
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true);
                }
                client.Dispose();
            }
        }
    }
}