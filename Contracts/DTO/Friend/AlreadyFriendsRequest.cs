using System.Text.Json.Serialization;

namespace Contracts.DTO.Friend;

public class AlreadyFriendsRequest
{
    [JsonPropertyName("userId")] public int UserId { get; init; }

    [JsonPropertyName("friendId")] public int FriendId { get; init; }
}