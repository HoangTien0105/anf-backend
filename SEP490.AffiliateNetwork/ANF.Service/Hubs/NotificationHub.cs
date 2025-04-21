using Microsoft.AspNetCore.SignalR;

namespace ANF.Service.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendCampaignStatusNoti(string message)
        {
            await Clients.All.SendAsync("Campaign status updated!", message); 
        }
    }
}
