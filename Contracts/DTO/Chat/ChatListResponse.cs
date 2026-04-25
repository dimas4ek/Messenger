using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Chat;

public class ChatListResponse
{
    [JsonPropertyName("chats")] public List<ChatInfo> Chats { get; set; }
}