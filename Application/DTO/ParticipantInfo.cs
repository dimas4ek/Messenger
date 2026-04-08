using Domain.Enums;

namespace Application.DTO;

public class ParticipantInfo
{
    public int ConversationId { get; set; }
    public int UserId { get; set; }
    public ParticipationRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
}