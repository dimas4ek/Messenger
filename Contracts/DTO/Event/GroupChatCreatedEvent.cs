using Application.DTO;

namespace Contracts.DTO.Event;

public class GroupChatCreatedEvent
{
    public required ChatInfo Chat { get; init; }
}