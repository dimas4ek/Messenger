using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Friend;

public class FriendRequestListResponse
{
    [JsonPropertyName("friendRequests")] public List<FriendRequestInfo> FriendRequests { get; set; }
}