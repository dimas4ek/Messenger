namespace Contracts.DTO.Chat;

public class SendMessageRequest
{
    public int CurrentUserId { get; set; }
    public int CompanionId { get; set; }
    public string Message { get; set; } = string.Empty;
}