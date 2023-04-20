using Microsoft.AspNetCore.SignalR;

namespace HandmadeShop.Hubs
{
    public class DefaultHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task JoinRoom(string roomId, string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("user-connected", userId);
        }

		public async Task SendMessage(string user,string message)
		{
			await Clients.All.SendAsync("ReceiveMessage",user, message);
		}

        public async Task Notification(string message)
        {
            await Clients.All.SendAsync("noti", message);
        }
    }
}
