using System.Text.Json.Serialization;

namespace Contracts.DTO.Friend;

public class SendFriendRequest
{
    [JsonPropertyName("senderId")] public int SenderId { get; init; }

    [JsonPropertyName("receiverId")] public int ReceiverId { get; init; }
}