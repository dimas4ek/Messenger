using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Friend;

public class FriendResponse
{
    [JsonPropertyName("friend")] public UserInfo Friend { get; set; }
}