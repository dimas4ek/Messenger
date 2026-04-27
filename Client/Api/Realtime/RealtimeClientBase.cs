using Client.Config;
using Client.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Api.Realtime;

public abstract class RealtimeClientBase(RemoteConfig remoteConfig, IDialogService dialogService)
{
    private HubConnection? _connection;

    protected abstract string HubPath { get; }
    protected abstract string UserGroupMethod { get; }

    protected abstract void RegisterHandlers(HubConnection connection);

    public async Task Connect(int currentUserId)
    {
        try
        {
            var hubUrl = $"{remoteConfig.ApiBaseUrl.TrimEnd('/')}/{HubPath}";

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options => { options.Transports = HttpTransportType.WebSockets; })
                .WithAutomaticReconnect()
                .Build();

            RegisterHandlers(_connection);

            try
            {
                if (_connection.State == HubConnectionState.Disconnected)
                    await _connection.StartAsync();
            }
            catch (Exception e)
            {
                dialogService.ShowError(e.Message);
            }

            if (_connection.State == HubConnectionState.Connected)
                await _connection.InvokeAsync(UserGroupMethod, currentUserId.ToString());

            _connection.Reconnected +=
                async _ => await _connection.InvokeAsync(UserGroupMethod, currentUserId.ToString());
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

    protected async Task InvokeAsync(string method, string arg)
    {
        if (_connection?.State == HubConnectionState.Connected)
            await _connection.InvokeAsync(method, arg);
    }
}