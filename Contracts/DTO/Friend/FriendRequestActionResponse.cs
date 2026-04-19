using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Friend;

public class FriendRequestActionResponse
{
    [JsonPropertyName("addedFriend")] public UserInfo AddedFriend { get; set; }
}