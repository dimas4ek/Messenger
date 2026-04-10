using System.Text.Json.Serialization;

namespace Contracts.DTO.Friend;

public class AddFriendRequest
{
    [JsonPropertyName("userId")] public int UserId { get; set; }

    [JsonPropertyName("friendId")] public int FriendId { get; set; }
}