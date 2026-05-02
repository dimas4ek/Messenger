using Client.Config;
using Client.Services;
using Contracts;
using Contracts.DTO.Event;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Api.Realtime;

public class FriendRealtimeClient(RemoteConfig remoteConfig, IDialogService dialogService)
    : RealtimeClientBase(remoteConfig, dialogService)
{
    protected override string HubPath => "friendHub";
    protected override string UserGroupMethod => HubMethods.Groups.JoinFriendGroup;

    public event Action<FriendAddedEvent>? FriendAdded;
    public event Action<FriendUpdatedEvent>? FriendUpdated;
    public event Action<FriendStatusEvent>? FriendStatus;

    protected override void RegisterHandlers(HubConnection connection)
    {
        connection.On<FriendAddedEvent>(HubMethods.Friends.FriendAdded, e => FriendAdded?.Invoke(e));
        connection.On<FriendUpdatedEvent>(HubMethods.Friends.FriendUpdated, e => FriendUpdated?.Invoke(e));
        connection.On<FriendStatusEvent>(HubMethods.Friends.FriendStatusUpdated, e => FriendStatus?.Invoke(e));
    }
}