using Application.DTO;

namespace Contracts.DTO.Friend.Event;

public class FriendUpdatedEvent
{
    public UserInfo User { get; set; }
    public ChatInfo FriendChat { get; set; }
    public bool UsernameChanged { get; set; }
    public bool AvatarChanged { get; set; }
}