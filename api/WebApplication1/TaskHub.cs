using Microsoft.AspNetCore.SignalR;

namespace WebApplication1
{
    public class TaskHub : Hub
    {
        public async Task SendTaskUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveTaskUpdate", message);
        }
    }
}
