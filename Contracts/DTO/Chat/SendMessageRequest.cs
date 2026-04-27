using System.Text.Json.Serialization;

namespace Contracts.DTO.Chat;

public class SendMessageRequest
{
    [JsonPropertyName("senderId")] public int SenderId { get; init; }
    [JsonPropertyName("text")] public string Text { get; init; } = string.Empty;
}