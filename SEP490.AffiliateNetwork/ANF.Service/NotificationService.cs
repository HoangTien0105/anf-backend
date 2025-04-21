using ANF.Core.Services;
using ANF.Service.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ANF.Service
{
    public class NotificationService(IHubContext<CampaignHub> campaignHubContext,
                                     IHubContext<OfferHub> offerHubContext) : INotificationService
    {
        private readonly IHubContext<CampaignHub> _campaignHubContext = campaignHubContext;
        private readonly IHubContext<OfferHub> _offerHubContext = offerHubContext;
        public async Task NotifyCampaignStatus(string userCode, long campaignId, string status, string? rejectReason)
        {
            await _campaignHubContext.Clients.User(userCode).SendAsync("CampaignStatusUpdated", new
            {
                UserCode = userCode,
                CampaignId = campaignId,
                Status = status,
                RejectReason = rejectReason
            });
        }

        public async Task NotifyOfferStatus(string userCode, long offerId, string status, string? rejectReason)
        {
            await _offerHubContext.Clients.User(userCode).SendAsync("OfferStatusUpdated", new
            {
                UserCode = userCode,
                OfferId = offerId,
                Status = status,
                RejectReason = rejectReason
            });
        }
    }
}
