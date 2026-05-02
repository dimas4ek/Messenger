using System.Text.Json.Serialization;

namespace Contracts.DTO.Chat;

public class EditChatRequest
{
    [JsonPropertyName("message")] public required string Name { get; init; }
}