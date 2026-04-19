using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("friend_requests")]
public class FriendRequest
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("sender_id")] public int SenderId { get; set; }
    [Column("receiver_id")] public int ReceiverId { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; }

    public User Sender { get; set; }
    public User Receiver { get; set; }
}