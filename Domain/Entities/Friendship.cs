using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("friend_list")]
public class Friendship
{
    [Column("user_id")] public int UserId { get; set; }

    public User User { get; set; }

    [Column("friend_id")] public int FriendId { get; set; }

    public User Friend { get; set; }
}