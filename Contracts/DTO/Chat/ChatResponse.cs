using Application.DTO;

namespace Contracts.DTO.Chat;

public class ChatResponse
{
    public ConversationInfo Conversation { get; set; }
}