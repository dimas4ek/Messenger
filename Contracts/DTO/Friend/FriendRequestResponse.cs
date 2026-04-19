using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Friend;

public class FriendRequestResponse
{
    [JsonPropertyName("friendRequest")] public FriendRequestInfo FriendRequest { get; set; }
}