using System.Text.Json.Serialization;

namespace Contracts.DTO.Friend;

public class SendFriendRequest
{
    [JsonPropertyName("senderId")] public int SenderId { get; set; }

    [JsonPropertyName("receiverId")] public int ReceiverId { get; set; }
}