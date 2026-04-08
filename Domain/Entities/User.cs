using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

[Table("users")]
public class User
{
    [Key] [Column("id")] public int Id { get; set; }

    [Column("username")] [MaxLength(255)] public required string Username { get; set; }

    [Column("password")] [MaxLength(255)] public string Password { get; set; }

    [Column("status")] public UserStatus Status { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; }

    public ICollection<ConversationParticipant> Participations { get; set; }
    public ICollection<Message> SentMessages { get; set; }
    public ICollection<Friendship> Friends { get; set; } = new List<Friendship>();
    public ICollection<Friendship> AddedByFriends { get; set; } = new List<Friendship>();
}