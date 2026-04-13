using Client.Config;
using Client.Service;
using Contracts.DTO.Chat;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Realtime;

public class ChatRealtimeClient(RemoteConfig remoteConfig, IDialogService dialogService)
{
    private HubConnection? _connection;

    public event Action<MessageResponse>? MessageReceived;
    public event Action<int>? MessageDeleted;

    public async Task Connect(int currentUserId)
    {
        try
        {
            var hubUrl = $"{remoteConfig.ApiBaseUrl.TrimEnd('/')}/chatHub";

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options => { options.Transports = HttpTransportType.WebSockets; })
                .WithAutomaticReconnect()
                .Build();

            _connection.On<MessageResponse>("ReceiveMessage", message => MessageReceived?.Invoke(message));
            _connection.On<int>("DeleteMessage", messageId => MessageDeleted?.Invoke(messageId));

            try
            {
                if (_connection.State == HubConnectionState.Disconnected) await _connection.StartAsync();
            }
            catch (Exception e)
            {
                dialogService.ShowError(e.Message);
            }

            if (_connection.State == HubConnectionState.Connected)
                await _connection.InvokeAsync("JoinUserGroup", currentUserId.ToString());

            _connection.Reconnected +=
                async _ => await _connection.InvokeAsync("JoinUserGroup", currentUserId.ToString());
        }
        catch (Exception e)
        {
            dialogService.ShowError(e.Message);
        }
    }

    public async Task Disconnect()
    {
        if (_connection != null)
            await _connection.DisposeAsync();
    }
}