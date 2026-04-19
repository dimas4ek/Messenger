namespace Application.DTO;

public class FriendRequestInfo
{
    public int Id { get; set; }
    public required UserInfo Sender { get; set; }
    public required UserInfo Receiver { get; set; }
    public DateTime CreatedAt { get; set; }
}