using Application.DTO;

namespace Contracts.DTO.Event;

public class FriendUpdatedEvent
{
    public required UserInfo User { get; init; }
    public bool UsernameChanged { get; init; }
    public bool AvatarChanged { get; init; }
}