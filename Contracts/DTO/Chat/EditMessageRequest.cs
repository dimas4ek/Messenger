using System.Text.Json.Serialization;

namespace Contracts.DTO.Chat;

public class EditMessageRequest
{
    [JsonPropertyName("message")] public required string Message { get; init; }
}