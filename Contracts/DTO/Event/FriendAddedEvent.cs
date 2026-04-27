using Application.DTO;

namespace Contracts.DTO.Event;

public class FriendAddedEvent
{
    public required UserInfo Friend { get; init; }
    public required ChatInfo FriendChat { get; init; }
}