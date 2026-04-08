namespace Contracts.DTO.Friend;

public class AlreadyFriendsRequest
{
    public int CurrentUserId { get; set; }
    public string FriendName { get; set; }
}