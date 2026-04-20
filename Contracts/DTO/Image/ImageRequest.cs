using System.Text.Json.Serialization;
using Domain.Enums;

namespace Contracts.DTO.Image;

public class ImageRequest
{
    [JsonPropertyName("contentType")] public ImageContentType ContentType;
    [JsonPropertyName("name")] public string Name;
    [JsonPropertyName("bytes")] public byte[] Bytes { get; set; }
}