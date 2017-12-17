using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Test_DB.Controllers
{
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.All.InvokeAsync("Send", message);
        }
    }
}