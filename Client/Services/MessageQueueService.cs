using Application.DTO;
using Client.Api.Clients;
using System.Collections;
using System.Threading.Channels;
using Client.Utils;

namespace Client.Services;

public class MessageQueueService(ChatApiClient chatApiClient, IDialogService dialogService) : IAsyncDisposable
{
    public sealed record OutgoingChatMessage(int CurrentChatId, int SenderId, string Text, Guid TempId);

    private readonly Channel<OutgoingChatMessage> _messageQueue =
        Channel.CreateBounded<OutgoingChatMessage>(new BoundedChannelOptions(100)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
        });

    private CancellationTokenSource? _cts;

    public event Action<OutgoingChatMessage, MessageInfo>? MessageConfirmed;

    public void Start()
    {
        _cts = new CancellationTokenSource();
        _ = ProcessAsync(_cts.Token);
    }

    public async ValueTask EnqueueAsync(OutgoingChatMessage message) =>
        await _messageQueue.Writer.WriteAsync(message);

    private async Task ProcessAsync(CancellationToken token)
    {
        try
        {
            await foreach (var item in _messageQueue.Reader.ReadAllAsync(token))
            {
                var result = await chatApiClient.SendMessage(item.CurrentChatId, item.SenderId, item.Text);
                if (result is { IsSuccess: true, Value: not null })
                    MessageConfirmed?.Invoke(item, result.Value.Message);
                else
                    dialogService.ShowError(result.ToMessage());
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_cts != null) await _cts.CancelAsync();
        _messageQueue.Writer.TryComplete();
    }
}