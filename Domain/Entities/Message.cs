using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("messages")]
public class Message
{
    [Key] [Column("id")] public int Id { get; set; }

    [Column("conversation_id")] public int ConversationId { get; set; }

    [Column("sender_id")] public int SenderId { get; set; }

    [Column("message_text")] public required string MessageText { get; set; }

    [Column("is_edited")] public bool IsEdited { get; set; }

    [Column("is_read")] public bool IsRead { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; }

    [Column("updated_at")] public DateTime UpdatedAt { get; set; }

    // Навигационные свойства
    public Conversation Conversation { get; set; }
    public User Sender { get; set; }
}