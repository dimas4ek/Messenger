using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Hubs;

public class ChatHub : Hub
{
    public async Task JoinUserGroup(string currentUserId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{currentUserId}");
    }

    public async Task JoinChatGroup(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"chat:{chatId}");
    }

    public async Task LeaveChatGroup(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"chat:{chatId}");
    }
}