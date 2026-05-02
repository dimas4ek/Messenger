using Application.DTO;

namespace Contracts.DTO.Event;

public class GroupChatUpdatedEvent
{
    public required ChatInfo Chat { get; init; }
    public bool NameChanged { get; init; }
}