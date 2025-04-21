using ANF.Core.Models.Entities;
using ANF.Core.Services;
using ANF.Service.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ANF.Service
{
    public class NotificationService(IHubContext<NotificationHub> hubContext) : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext = hubContext;
        public async Task NotifyCampaignStatus(string userCode, long campaignId, string status, string? rejectReason)
        {
            await _hubContext.Clients.User(userCode).SendAsync("CampaignStatusUpdated", new
            {
                UserCode = userCode,
                CampaignId = campaignId,
                Status = status,
                RejectReason = rejectReason
            });
        }

        public async Task NotifyOfferStatus(string userCode, long offerId, string status, string? rejectReason)
        {
            await _hubContext.Clients.User(userCode).SendAsync("OfferStatusUpdated", new
            {
                UserCode = userCode,
                OfferId = offerId,
                Status = status,
                RejectReason = rejectReason
            });
        }

        public async Task NotifyPublisherOffer(string userCode, long pubOfferId, string status, string? rejectReason)
        {
            await _hubContext.Clients.User(userCode).SendAsync("PublisherOfferStatusUpdated", new
            {
                UserCode = userCode,
                OfferId = pubOfferId,
                Status = status,
                RejectReason = rejectReason
            });
        }

        public async Task NotifyUserProfile(string userCode)
        {
            await _hubContext.Clients.User(userCode).SendAsync("UserProfileUpdated", new
            {
                UserCode = userCode
            });
        }
    }
}
