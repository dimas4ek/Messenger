using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Hubs;

public class FriendHub : Hub
{
    public async Task JoinFriendGroup(string currentUserId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{currentUserId}");
    }
}