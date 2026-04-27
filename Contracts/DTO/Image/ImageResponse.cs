using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Image;

public class ImageResponse
{
    [JsonPropertyName("image")] public required ImageInfo Image { get; init; }
}