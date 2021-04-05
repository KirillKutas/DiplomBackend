using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Backend
{
    public class WSHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("Send", message);
        }
    }
}
