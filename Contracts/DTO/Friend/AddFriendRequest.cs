using Application.DTO;

namespace Contracts.DTO.Friend;

public class AddFriendRequest
{
    public int CurrentUserId { get; set; }
    public UserInfo Friend { get; set; }
}