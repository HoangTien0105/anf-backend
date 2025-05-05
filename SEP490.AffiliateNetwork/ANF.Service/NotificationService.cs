using ANF.Core.Models.Entities;
using ANF.Core.Services;
using ANF.Service.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ANF.Service
{
    public class NotificationService(IHubContext<NotificationHub> hubContext) : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext = hubContext;

        public async Task NotifyCampaignCreated(string message)
        {
            await _hubContext.Clients.Group("Admins").SendAsync("CampaignCreated", message);
        }

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

        public async Task NotifyPublisherOffer(string userCode, long pubOfferId, string status, string? rejectReason, long campaignId)
        {
            await _hubContext.Clients.User(userCode).SendAsync("PublisherOfferStatusUpdated", new
            {
                UserCode = userCode,
                OfferId = pubOfferId,
                Status = status,
                RejectReason = rejectReason,
                CampaignId = campaignId
            });
        }

        public async Task NotifyRequestToJoinOffer(string userCode, string message, long campaignId, long offerId)
        {
            await _hubContext.Clients.User(userCode).SendAsync("NotifyRequestToJoinOffer", new
            {
                UserCode = userCode,
                Message = message,
                CampaignId = campaignId,
                OfferId = offerId
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
