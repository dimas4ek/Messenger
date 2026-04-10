using Domain.Enums;

namespace Application.DTO;

public class ChatParticipantInfo
{
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public ChatParticipationRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
}