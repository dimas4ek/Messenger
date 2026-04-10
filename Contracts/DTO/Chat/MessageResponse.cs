using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Chat;

public class MessageResponse
{
    [JsonPropertyName("message")] public MessageInfo Message { get; set; }
}