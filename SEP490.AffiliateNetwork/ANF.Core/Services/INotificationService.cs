namespace ANF.Core.Services
{
    public interface INotificationService
    {
        Task NotifyCampaignStatus(string userCode, long campaignId, string status, string? rejectReason);
    }
}
