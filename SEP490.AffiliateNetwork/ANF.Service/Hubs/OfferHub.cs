using Microsoft.AspNetCore.SignalR;

namespace ANF.Service.Hubs
{
    public class OfferHub : Hub
    {
        public async Task SendOfferStatusNoti(string message)
        {
            await Clients.All.SendAsync("Offer status updated!", message);
        }
    }
}
