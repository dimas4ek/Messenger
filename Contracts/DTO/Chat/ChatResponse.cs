using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Chat;

public class ChatResponse
{
    [JsonPropertyName("chat")] public ChatInfo Chat { get; set; }
}