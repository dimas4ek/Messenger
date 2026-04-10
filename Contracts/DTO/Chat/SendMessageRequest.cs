using System.Text.Json.Serialization;

namespace Contracts.DTO.Chat;

public class SendMessageRequest
{
    [JsonPropertyName("senderId")] public int SenderId { get; set; }

    [JsonPropertyName("companionId")] public int CompanionId { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
}