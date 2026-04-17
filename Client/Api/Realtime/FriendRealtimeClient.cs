using Client.Config;
using Client.Services;
using Contracts.DTO.User;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Api.Realtime;

public class FriendRealtimeClient(RemoteConfig remoteConfig, IDialogService dialogService)
{
    private HubConnection? _connection;

    public event Action<UserResponse>? FriendAdded;
    public event Action<UserResponse>? FriendUpdated;

    public async Task Connect(int currentUserId)
    {
        try
        {
            var hubUrl = $"{remoteConfig.ApiBaseUrl.TrimEnd('/')}/friendHub";

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options => { options.Transports = HttpTransportType.WebSockets; })
                .WithAutomaticReconnect()
                .Build();

            _connection.On<UserResponse>("FriendAdded", message => FriendAdded?.Invoke(message));
            _connection.On<UserResponse>("FriendUpdated", message => FriendUpdated?.Invoke(message));

            try
            {
                if (_connection.State == HubConnectionState.Disconnected) await _connection.StartAsync();
            }
            catch (Exception e)
            {
                dialogService.ShowError(e.Message);
            }

            if (_connection.State == HubConnectionState.Connected)
                await _connection.InvokeAsync("JoinFriendGroup", currentUserId.ToString());

            _connection.Reconnected +=
                async _ => await _connection.InvokeAsync("JoinFriendGroup", currentUserId.ToString());
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