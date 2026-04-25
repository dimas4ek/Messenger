using Domain.Enums;

namespace Application.DTO;

public class ChatInfo
{
    public int Id { get; set; }
    public ChatType Type { get; set; }
    public string Name { get; set; }
    public ImageInfo? Image { get; set; }
    public List<MessageInfo>? Messages { get; set; }
    public required List<ChatParticipantInfo> Participants { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}