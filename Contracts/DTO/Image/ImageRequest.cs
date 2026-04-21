using System.Text.Json.Serialization;
using Domain.Enums;

namespace Contracts.DTO.Image;

public class ImageRequest
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("bytes")] public byte[] Bytes { get; set; }
    [JsonPropertyName("contentType")] public ImageContentType ContentType { get; set; }
}