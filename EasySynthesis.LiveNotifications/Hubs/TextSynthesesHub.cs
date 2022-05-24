using Microsoft.AspNetCore.SignalR;

namespace EasySynthesis.LiveNotifications.Hubs;

public class TextSynthesesHub : Hub
{
    public Task BroadcastMessage(string name, string message) =>
        Clients.All.SendAsync("broadcastMessage", name, message);

    public Task Echo(string name, string message) =>
        Clients.Client(Context.ConnectionId)
            .SendAsync("echo", name, $"{message} (echo from server)");
    
    public async Task SendMessage(string username, string message)
    {
        await Clients.All.SendAsync("messageReceived", username, message);
    }
}