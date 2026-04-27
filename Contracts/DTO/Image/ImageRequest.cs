using System.Text.Json.Serialization;
using Domain.Enums;

namespace Contracts.DTO.Image;

public class ImageRequest
{
    [JsonPropertyName("name")] public required string Name { get; init; }
    [JsonPropertyName("bytes")] public required byte[] Bytes { get; init; }
    [JsonPropertyName("contentType")] public ImageContentType ContentType { get; init; }
}