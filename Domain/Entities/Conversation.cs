using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

[Table("conversations")]
public class Conversation
{
    [Key] [Column("id")] public int Id { get; set; }

    [Column("type")] public ConversationType Type { get; set; }

    [Column("name")] [MaxLength(255)] public string? Name { get; set; } // null для приватных чатов

    [Column("created_at")] public DateTime CreatedAt { get; set; }

    [Column("updated_at")] public DateTime UpdatedAt { get; set; }

    public ICollection<ConversationParticipant> Participants { get; set; } = new List<ConversationParticipant>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}