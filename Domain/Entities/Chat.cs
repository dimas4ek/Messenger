using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

[Table("chats")]
public class Chat
{
    [Key] [Column("id")] public int Id { get; set; }

    [Column("type")] public ChatType Type { get; set; }

    [Column("name")] [MaxLength(255)] public string? Name { get; set; } // null для приватных чатов

    [Column("created_at")] public DateTime CreatedAt { get; set; }

    [Column("updated_at")] public DateTime UpdatedAt { get; set; }

    public ICollection<ChatParticipant> Participants { get; set; } = new List<ChatParticipant>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}