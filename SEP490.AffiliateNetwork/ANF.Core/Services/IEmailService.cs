using ANF.Core.Commons;

namespace ANF.Core.Services
{
    public interface IEmailService
    {
        Task<bool> SendVerificationEmail(EmailMessage message, string verificationUrl);

        Task<bool> SendTokenForResetPassword(EmailMessage message, string resetUrl);
    }
}
