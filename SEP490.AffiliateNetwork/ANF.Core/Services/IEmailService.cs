using ANF.Core.Commons;

namespace ANF.Core.Services
{
    public interface IEmailService
    {
        Task<bool> SendVerificationEmail(EmailMessage message, string verificationUrl);

        Task<bool> SendTokenForResetPassword(EmailMessage message, string resetUrl);
        Task<bool> SendCampaignNotificationEmail(EmailMessage message, string campaignName, long campaignId,long? offer, string status);
        Task<bool> SendNotificationEmailForAdvertiser(EmailMessage message, long campaignId, long offerId);
        Task<bool> SendNotificationEmailForPublisher(EmailMessage message, long campaignId);
    }
}
