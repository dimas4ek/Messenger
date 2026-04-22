using Application.DTO;

namespace Contracts.DTO.Friend.Event;

public class FriendStatusEvent
{
    public UserInfo User { get; set; }
    public bool StatusChanged { get; set; }
}