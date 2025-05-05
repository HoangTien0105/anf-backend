namespace ANF.Core.Services
{
    public interface INotificationService
    {
        Task NotifyCampaignStatus(string userCode, long campaignId, string status, string? rejectReason);
        Task NotifyOfferStatus(string userCode, long offerId, string status, string? rejectReason);
        Task NotifyUserProfile(string userCode);
        //Notify pub about their join request
        Task NotifyPublisherOffer(string userCode, long pubOfferId, string status, string? rejectReason, long campaignId);
        //Notify adv about their offer has new join request
        Task NotifyRequestToJoinOffer(string userCode, string message, long campaignId, long offerId);
        Task NotifyCampaignCreated(string message);
    }
}   
