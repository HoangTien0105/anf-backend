namespace ANF.Core.Services
{
    public interface INotificationService
    {
        Task NotifyCampaignStatus(string userCode, long campaignId, string status, string? rejectReason);
        Task NotifyOfferStatus(string userCode, long offerId, string status, string? rejectReason);
        Task NotifyUserProfile(string userCode);
    }
}
