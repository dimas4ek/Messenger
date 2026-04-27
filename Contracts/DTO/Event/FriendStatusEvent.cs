using Application.DTO;

namespace Contracts.DTO.Event;

public class FriendStatusEvent
{
    public required UserInfo User { get; init; }
    public bool StatusChanged { get; init; }
}