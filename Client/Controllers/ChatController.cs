using Application.DTO;
using Client.Api.Clients;
using Client.Api.Realtime;
using Client.Services;
using Client.Utils;
using Contracts.DTO.Chat;
using Contracts.DTO.Event;

namespace Client.Controllers;

public class ChatController
{
    private readonly ChatApiClient _chatApiClient;
    private readonly ChatRealtimeClient _chatRealtimeClient;
    private readonly IDialogService _dialogService;

    public ChatController(ChatApiClient chatApiClient, ChatRealtimeClient chatRealtimeClient,
        IDialogService dialogService)
    {
        _chatApiClient = chatApiClient;
        _chatRealtimeClient = chatRealtimeClient;
        _dialogService = dialogService;

        _chatRealtimeClient.GroupChatCreated += OnGroupChatCreated;

        _chatRealtimeClient.MessageReceived += OnMessageReceived;
        _chatRealtimeClient.MessageUpdated += OnMessageUpdated;
        _chatRealtimeClient.MessageDeleted += OnMessageDeleted;
    }

    public List<ChatInfo> Chats { get; private set; } = [];
    public ChatInfo? CurrentChat { get; private set; }
    private List<MessageInfo>? Messages { get; set; } = [];

    public event Action<GroupChatCreatedEvent>? GroupChatCreated;

    public event Action<MessageInfo>? MessageLoaded;
    public event Action<MessageInfo>? MessageReceived;
    public event Action<MessageInfo>? MessageUpdated;
    public event Action<int>? MessageDeleted;

    public async Task LoadChats(int userId)
    {
        var result = await _chatApiClient.GetChatList(userId);
        if (!result.IsSuccess || result.Value == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        Chats = result.Value.Chats;
    }

    public async Task LoadChat(int currentUserId, int chatId)
    {
        if (CurrentChat != null)
            await _chatRealtimeClient.LeaveChat(CurrentChat.Id);

        var result = await _chatApiClient.LoadChat(currentUserId, chatId);
        if (!result.IsSuccess || result.Value == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        CurrentChat = result.Value.Chat;
        Messages = CurrentChat.Messages ?? [];

        foreach (var msg in Messages)
            MessageLoaded?.Invoke(msg);

        await _chatRealtimeClient.JoinChat(CurrentChat.Id);
    }

    public async Task EditMessage(MessageInfo message, string newText)
    {
        if (CurrentChat == null) return;

        var result = await _chatApiClient.EditMessage(CurrentChat.Id, message.Id, newText);
        if (!result.IsSuccess)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        message.Text = newText;
        MessageUpdated?.Invoke(message);
    }

    public async Task DeleteMessage(MessageInfo message)
    {
        if (CurrentChat == null) return;

        var result = await _chatApiClient.DeleteMessage(CurrentChat.Id, message.Id);
        if (!result.IsSuccess)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        Messages?.Remove(message);
        MessageDeleted?.Invoke(message.Id);
    }

    public List<ChatInfo> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return Chats;

        return Chats
            .Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public async Task ConnectAsync(int userId)
    {
        await _chatRealtimeClient.Connect(userId);
    }

    public async Task DisconnectAsync()
    {
        await _chatRealtimeClient.Disconnect();
    }

    private void OnGroupChatCreated(GroupChatCreatedEvent e)
    {
        GroupChatCreated?.Invoke(e);
    }

    private void OnMessageReceived(MessageResponse r)
    {
        if (CurrentChat == null || r.Message.ChatId != CurrentChat.Id) return;
        MessageReceived?.Invoke(r.Message);
    }

    private void OnMessageUpdated(MessageResponse r)
    {
        if (CurrentChat == null || r.Message.ChatId != CurrentChat.Id) return;
        MessageUpdated?.Invoke(r.Message);
    }

    private void OnMessageDeleted(int id)
    {
        if (CurrentChat == null) return;
        MessageDeleted?.Invoke(id);
    }
}