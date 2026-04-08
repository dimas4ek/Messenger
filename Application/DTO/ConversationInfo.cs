using Domain.Enums;

namespace Application.DTO;

public class ConversationInfo
{
    public int Id { get; set; }
    public ConversationType Type { get; set; }
    public string? Name { get; set; }
    public List<MessageInfo>? Messages { get; set; }
    public required List<ParticipantInfo> Participants { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}