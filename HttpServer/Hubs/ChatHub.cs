using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Hubs;

public class ChatHub : Hub
{
    public async Task JoinUserGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
    }
}