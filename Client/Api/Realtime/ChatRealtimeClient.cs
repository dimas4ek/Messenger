using Client.Config;
using Client.Services;
using Contracts.DTO.Chat;
using Contracts.DTO.Chat.Event;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Api.Realtime;

public class ChatRealtimeClient(RemoteConfig remoteConfig, IDialogService dialogService)
    : RealtimeClientBase(remoteConfig, dialogService)
{
    protected override string HubPath => "chatHub";
    protected override string UserGroupMethod => "JoinUserGroup";
    public event Action<GroupChatCreatedEvent>? GroupChatCreated;
    public event Action<MessageResponse>? MessageReceived;
    public event Action<MessageResponse>? MessageUpdated;
    public event Action<int>? MessageDeleted;

    protected override void RegisterHandlers(HubConnection connection)
    {
        connection.On<GroupChatCreatedEvent>("GroupChatCreated", e => GroupChatCreated?.Invoke(e));
        connection.On<MessageResponse>("ReceiveMessage", message => MessageReceived?.Invoke(message));
        connection.On<MessageResponse>("EditMessage", message => MessageUpdated?.Invoke(message));
        connection.On<int>("DeleteMessage", messageId => MessageDeleted?.Invoke(messageId));
    }

    public Task JoinChat(int chatId)
    {
        return InvokeAsync("JoinChatGroup", chatId.ToString());
    }

    public Task LeaveChat(int chatId)
    {
        return InvokeAsync("LeaveChatGroup", chatId.ToString());
    }
}