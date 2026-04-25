using Application.DTO;

namespace Contracts.DTO.Chat.Event;

public class GroupChatCreatedEvent
{
    public ChatInfo Chat { get; set; }
}