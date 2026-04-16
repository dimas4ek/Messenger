using Application.DTO;
using Client.Api.Clients;
using Client.Api.Realtime;
using Client.Services;
using Client.Utils;
using Contracts.DTO.Chat;

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

        _chatRealtimeClient.MessageReceived += OnMessageReceived;
        _chatRealtimeClient.MessageUpdated += OnMessageUpdated;
        _chatRealtimeClient.MessageDeleted += OnMessageDeleted;
    }

    public ChatInfo? CurrentChat { get; private set; }
    public List<MessageInfo>? Messages { get; private set; } = [];


    public event Action<MessageInfo>? MessageLoaded;
    public event Action<MessageInfo>? MessageReceived;
    public event Action<MessageInfo>? MessageUpdated;
    public event Action<int>? MessageDeleted;

    public async Task LoadDialogAsync(int currentUserId, UserInfo companion)
    {
        if (CurrentChat != null)
            await _chatRealtimeClient.LeaveChat(CurrentChat.Id);

        var result = await _chatApiClient.LoadPrivateChat(currentUserId, companion.Id);
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

    public async Task EditAsync(MessageInfo message, string newText)
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

    public async Task DeleteAsync(MessageInfo message)
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

    public async Task ConnectAsync(int userId)
    {
        await _chatRealtimeClient.Connect(userId);
    }

    public async Task DisconnectAsync()
    {
        await _chatRealtimeClient.Disconnect();
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