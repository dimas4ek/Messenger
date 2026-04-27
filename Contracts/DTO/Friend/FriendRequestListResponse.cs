using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Friend;

public class FriendRequestListResponse
{
    [JsonPropertyName("friendRequests")] public required List<FriendRequestInfo> FriendRequests { get; init; }
}