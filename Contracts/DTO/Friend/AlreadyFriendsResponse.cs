using System.Text.Json.Serialization;

namespace Contracts.DTO.Friend;

public class AlreadyFriendsResponse
{
    [JsonPropertyName("isFriends")] public bool IsFriends { get; init; }
}