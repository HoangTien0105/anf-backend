using ANF.Core.Commons;
using ANF.Core.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ANF.Service
{
    public class EmailService(IOptions<EmailSettings> options) : IEmailService
    {
        private readonly EmailSettings _options = options.Value;
        
        public async Task<bool> SendTokenForResetPassword(EmailMessage message, string resetUrl)
        {
            var result = false;
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(_options.SenderName, _options.SenderEmail));
                mimeMessage.To.Add(new MailboxAddress("", message.To));
                mimeMessage.Subject = message.Subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = $@"<h2>Reset password</h2>
                        <p>Please click the link below to reset the password:</p>
                        <a href='{resetUrl}'>Reset password</a>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(_options.SmtpServer, _options.Port,
                    SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_options.SenderEmail, _options.Password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);

                result = true;
            }
            catch
            {
                throw;
            }
            return result;
        }

        public async Task<bool> SendVerificationEmail(EmailMessage message, string verificationUrl)
        {
            var result = false;
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(_options.SenderName, _options.SenderEmail));
                mimeMessage.To.Add(new MailboxAddress("", message.To));
                mimeMessage.Subject = message.Subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = $@"<h2>Account Verification</h2>
                        <p>Please click the link below to verify your account:</p>
                        <a href='{verificationUrl}'>Verify account</a>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(_options.SmtpServer, _options.Port,
                    SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_options.SenderEmail, _options.Password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);

                result = true;
            }
            catch
            {
                throw;
            }
            return result;
        }
    }
}
