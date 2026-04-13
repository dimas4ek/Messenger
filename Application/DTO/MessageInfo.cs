namespace Application.DTO;

public class MessageInfo
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public required UserInfo Sender { get; set; }
    public required int ChatId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsEdited { get; set; }
    public bool IsRead { get; set; }

    public Guid TempId { get; set; }
}