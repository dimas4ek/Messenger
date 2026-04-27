using System.Text.Json.Serialization;

namespace Contracts.DTO.Chat;

public class GroupChatRequest
{
    [JsonPropertyName("name")] public required string Name { get; init; }
    [JsonPropertyName("imageId")] public int? ImageId { get; init; }
    [JsonPropertyName("creatorId")] public int CreatorId { get; init; }
    [JsonPropertyName("addedUserIds")] public required IEnumerable<int> AddedUserIds { get; init; }
}