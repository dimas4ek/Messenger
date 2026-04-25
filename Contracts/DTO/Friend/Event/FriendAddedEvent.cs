using Application.DTO;

namespace Contracts.DTO.Friend.Event;

public class FriendAddedEvent
{
    public UserInfo Friend { get; set; }
    public ChatInfo FriendChat { get; set; }
}