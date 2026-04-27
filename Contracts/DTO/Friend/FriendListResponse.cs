using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Friend;

public class FriendListResponse
{
    [JsonPropertyName("friends")] public required List<UserInfo> Friends { get; init; }
}