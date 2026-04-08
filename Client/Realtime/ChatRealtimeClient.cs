using Client.Config;
using Contracts.DTO.Chat;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Realtime;

public class ChatRealtimeClient
{
    private readonly RemoteConfig _remoteConfig;
    private HubConnection? _connection;

    public ChatRealtimeClient(RemoteConfig remoteConfig)
    {
        _remoteConfig = remoteConfig;
    }

    public event Action<MessageResponse>? MessageReceived;

    public async Task Connect(int currentUserId)
    {
        try
        {
            var hubUrl = $"{_remoteConfig.ApiBaseUrl.TrimEnd('/')}/chatHub";

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options => { options.Transports = HttpTransportType.WebSockets; })
                .WithAutomaticReconnect()
                .Build();

            _connection.On<MessageResponse>("ReceiveMessage", message => MessageReceived?.Invoke(message));

            try
            {
                if (_connection.State == HubConnectionState.Disconnected) await _connection.StartAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            if (_connection.State == HubConnectionState.Connected)
                await _connection.InvokeAsync("JoinUserGroup", currentUserId.ToString());

            _connection.Reconnected +=
                async _ => await _connection.InvokeAsync("JoinUserGroup", currentUserId.ToString());
        }
        catch (Exception e)
        {
            ;
            MessageBox.Show(e.ToString());
            throw;
        }
    }

    public async Task Disconnect()
    {
        if (_connection != null)
            await _connection.DisposeAsync();
    }
}