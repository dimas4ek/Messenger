using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Friend;

public class FriendRequestActionResponse
{
    [JsonPropertyName("friend")] public required UserInfo Friend { get; init; }
    [JsonPropertyName("createdChat")] public required ChatInfo CreatedChat { get; init; }
}