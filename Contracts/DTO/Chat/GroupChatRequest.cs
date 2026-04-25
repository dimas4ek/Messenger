using System.Text.Json.Serialization;

namespace Contracts.DTO.Chat;

public class GroupChatRequest
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("imageId")] public int? ImageId { get; set; }
    [JsonPropertyName("creatorId")] public int CreatorId { get; set; }
    [JsonPropertyName("addedUserIds")] public IEnumerable<int> AddedUserIds { get; set; }
}