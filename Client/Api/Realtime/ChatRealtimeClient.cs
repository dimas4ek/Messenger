using Client.Config;
using Client.Services;
using Contracts;
using Contracts.DTO.Chat;
using Contracts.DTO.Event;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Api.Realtime;

public class ChatRealtimeClient(RemoteConfig remoteConfig, IDialogService dialogService)
    : RealtimeClientBase(remoteConfig, dialogService)
{
    protected override string HubPath => "chatHub";
    protected override string UserGroupMethod => HubMethods.Groups.JoinUserGroup;

    public event Action<GroupChatCreatedEvent>? GroupChatCreated;
    public event Action<GroupChatUpdatedEvent>? GroupChatUpdated;
    public event Action<int>? ChatDeleted;
    public event Action<MessageResponse>? MessageReceived;
    public event Action<MessageResponse>? MessageUpdated;
    public event Action<int>? MessageDeleted;

    protected override void RegisterHandlers(HubConnection connection)
    {
        connection.On<GroupChatCreatedEvent>(HubMethods.Chats.GroupChatCreated, e => GroupChatCreated?.Invoke(e));
        connection.On<GroupChatUpdatedEvent>(HubMethods.Chats.GroupChatUpdated, e => GroupChatUpdated?.Invoke(e));
        connection.On<int>(HubMethods.Chats.GroupChatDeleted, chatId => ChatDeleted?.Invoke(chatId));
        connection.On<MessageResponse>(HubMethods.Messages.MessageReceived, message => MessageReceived?.Invoke(message));
        connection.On<MessageResponse>(HubMethods.Messages.MessageUpdated, message => MessageUpdated?.Invoke(message));
        connection.On<int>(HubMethods.Messages.MessageDeleted, messageId => MessageDeleted?.Invoke(messageId));
    }

    public Task JoinChat(int chatId)
    {
        return InvokeAsync(HubMethods.Groups.JoinChatGroup, chatId.ToString());
    }

    public Task LeaveChat(int chatId)
    {
        return InvokeAsync(HubMethods.Groups.LeaveChatGroup, chatId.ToString());
    }
}