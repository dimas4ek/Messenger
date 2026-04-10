using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Friend;

public class FriendListResponse
{
    [JsonPropertyName("friends")] public List<UserInfo> Friends { get; set; }
}