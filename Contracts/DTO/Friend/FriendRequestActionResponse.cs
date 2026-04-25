using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Friend;

public class FriendRequestActionResponse
{
    [JsonPropertyName("friend")] public UserInfo Friend { get; set; }
    [JsonPropertyName("createdChat")] public ChatInfo CreatedChat { get; set; }
}