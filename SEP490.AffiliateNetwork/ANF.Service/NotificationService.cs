using ANF.Core.Services;
using ANF.Service.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ANF.Service
{
    public class NotificationService(IHubContext<CampaignHub> hubContext) : INotificationService
    {
        private readonly IHubContext<CampaignHub> _hubContext;
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
    }
}
