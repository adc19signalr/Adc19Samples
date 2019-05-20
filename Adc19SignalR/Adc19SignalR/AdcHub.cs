using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Adc19SignalR
{
    public class AdcHub : Hub
    {
        public Task BroadcastMessage(string msg)
        {
            return Clients.All.SendAsync("EmpfangeNachricht", msg);
        }
    }
}
