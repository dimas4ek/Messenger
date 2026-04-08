using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

[Table("conversation_participants")]
public class ConversationParticipant
{
    [Column("conversation_id")] public int ConversationId { get; set; }

    [Column("participant_id")] public int ParticipantId { get; set; }

    [Column("role")] public ParticipationRole Role { get; set; }

    [Column("joined_at")] public DateTime JoinedAt { get; set; }

    public Conversation Conversation { get; set; }
    public User Participant { get; set; }
}