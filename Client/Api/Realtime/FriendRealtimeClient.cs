using Client.Config;
using Client.Services;
using Contracts.DTO.Event;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Api.Realtime;

public class FriendRealtimeClient(RemoteConfig remoteConfig, IDialogService dialogService)
    : RealtimeClientBase(remoteConfig, dialogService)
{
    protected override string HubPath => "friendHub";
    protected override string UserGroupMethod => "JoinFriendGroup";
    public event Action<FriendAddedEvent>? FriendAdded;
    public event Action<FriendUpdatedEvent>? FriendUpdated;
    public event Action<FriendStatusEvent>? FriendStatus;

    protected override void RegisterHandlers(HubConnection connection)
    {
        connection.On<FriendAddedEvent>("FriendAdded", e => FriendAdded?.Invoke(e));
        connection.On<FriendUpdatedEvent>("FriendUpdated", e => FriendUpdated?.Invoke(e));
        connection.On<FriendStatusEvent>("FriendStatus", e => FriendStatus?.Invoke(e));
    }
}