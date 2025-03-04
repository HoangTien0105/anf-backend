using ANF.Core.Commons;
using ANF.Core.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
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
                    Text = $@"
                        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; box-shadow: 2px 2px 10px rgba(0,0,0,0.1); background-color: #f9f9f9;'>
                            <h2 style='color: #d9534f; text-align: center;'>Reset Your Password</h2>
                            <p style='font-size: 16px; color: #555; text-align: center;'>We received a request to reset your password. Click the button below to proceed.</p>
                            <div style='text-align: center; margin-top: 20px;'>
                                <a href='{resetUrl}' style='display: inline-block; padding: 12px 24px; font-size: 16px; color: #fff; background-color: #d9534f; text-decoration: none; border-radius: 5px; font-weight: bold;'>
                                    Reset Password
                                </a>
                            </div>
                            <p style='font-size: 14px; color: #888; text-align: center; margin-top: 20px;'>If you did not request a password reset, please ignore this email.</p>
                        </div>"
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
                    Text = $@"
                        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; box-shadow: 2px 2px 10px rgba(0,0,0,0.1);'>
                            <h2 style='color: #4CAF50; text-align: center;'>Account Verification</h2>
                            <p style='font-size: 16px; color: #555; text-align: center;'>You're almost there! Click the button below to verify your account and get started.</p>
                            <div style='text-align: center; margin-top: 20px;'>
                                <a href='{verificationUrl}' style='display: inline-block; padding: 12px 24px; font-size: 16px; color: #fff; background-color: #4CAF50; text-decoration: none; border-radius: 5px; font-weight: bold;'>
                                    Verify Account
                                </a>
                            </div>
                            <p style='font-size: 14px; color: #888; text-align: center; margin-top: 20px;'>If you did not request this, please ignore this email.</p>
                        </div>"
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
