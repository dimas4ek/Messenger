using Application.DTO;

namespace Contracts.DTO.Friend;

public class FriendListResponse
{
    public List<UserInfo> Friends { get; set; }
}