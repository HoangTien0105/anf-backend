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

        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            var isAdmin = user?.IsInRole("Admin") ?? false;

            if (isAdmin)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
