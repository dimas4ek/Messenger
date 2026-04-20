using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Image;

public class ImageResponse
{
    [JsonPropertyName("image")] public ImageInfo Image { get; set; }
}