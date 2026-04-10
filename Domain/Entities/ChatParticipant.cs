using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

[Table("chat_participants")]
public class ChatParticipant
{
    [Column("chat_id")] public int ChatId { get; set; }

    [Column("participant_id")] public int ParticipantId { get; set; }

    [Column("role")] public ChatParticipationRole Role { get; set; }

    [Column("joined_at")] public DateTime JoinedAt { get; set; }

    public Chat Chat { get; set; }
    public User Participant { get; set; }
}