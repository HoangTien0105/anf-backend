using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ANF.Service.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public async Task SendCampaignStatusNoti(string message)
        {
            await Clients.All.SendAsync("CampaignStatusUpdated", message); 
        }
    }
}
