using Domain.Enums;

namespace Application.Utils;

public static class ImageContentTypeExtensions
{
    public static string ToMimeString(this ImageContentType type)
    {
        return type switch
        {
            ImageContentType.Jpeg => "image/jpeg",
            ImageContentType.Png => "image/png",
            ImageContentType.Gif => "image/gif",
            ImageContentType.Webp => "image/webp",
            ImageContentType.Bmp => "image/bmp",
            ImageContentType.Svg => "image/svg+xml",
            ImageContentType.Tiff => "image/tiff",
            ImageContentType.Ico => "image/ico",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    public static ImageContentType FromExtension(string filePath)
    {
        return Path.GetExtension(filePath).ToLower() switch
        {
            ".jpg" or ".jpeg" => ImageContentType.Jpeg,
            ".png" => ImageContentType.Png,
            ".gif" => ImageContentType.Gif,
            ".webp" => ImageContentType.Webp,
            ".bmp" => ImageContentType.Bmp,
            ".svg" => ImageContentType.Svg,
            ".tiff" or ".tif" => ImageContentType.Tiff,
            ".ico" => ImageContentType.Ico,
            _ => throw new NotSupportedException($"Неизвестный формат: {filePath}")
        };
    }
}