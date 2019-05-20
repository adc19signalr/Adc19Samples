using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Adc19SignalR
{
    public class AdcHub : Hub
    {
        // Im Beispiel werden nur Strings verwendet, allerdings kann jedes C# Objekt verwendet werden, solange es in JSON serialisiert werden kann

        // Sendet eine Nachricht an alle verbundenen Clients
        public Task BroadcastMessage(string msg)
        {
            return Clients.All.SendAsync("EmpfangeNachricht", msg);
        }

        // Sendet eine Nachricht zurück an sich selbst
        public async Task SendToCaller(string msg)
        {
            await Clients.Caller.SendAsync("EmpfangeNachricht", msg);
        }

        // Sendet eine Nachricht an alle verbundenen Clients, ausgenommen des aufrufenden Clients
        public async Task SendToOthers(string msg)
        {
            await Clients.Others.SendAsync("EmpfangeNachricht", msg);
        }

        // Sendet die Nachricht an alle Clients, die zur Gruppe hinzugefügt wurden
        public async Task SendToGroup(string groupName, string msg)
        {
            await Clients.Group(groupName).SendAsync("EmpfangeNachricht", msg);
        }

        // Fügt den aufrufenden Client zu einer spezifischen Gruppe hinzu
        public async Task AddUserToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("EmpfangeNachricht", $"Aktueller Benutzer zur {groupName} Gruppe hinzugefügt");
            await Clients.Others.SendAsync("EmpfangeNachricht", $"Benutzer {Context.ConnectionId} zur {groupName} Gruppe hinzugefügt");
        }

        // Entfernt den Client von der aufrufenden Gruppe
        public async Task RemoveUserFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("EmpfangeNachricht", $"Aktueller Benutzer von der {groupName} Gruppe entfernt");
            await Clients.Others.SendAsync("EmpfangeNachricht", $"Benutzer {Context.ConnectionId} von der {groupName} Gruppe entfernt");
        }

        // override eventhandler connect/disconnect
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "HubUsers");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "HubUsers");
            await base.OnDisconnectedAsync(ex);
        }
    }
}
