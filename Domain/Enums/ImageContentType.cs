using System.ComponentModel;

namespace Domain.Enums;

public enum ImageContentType
{
    [Description("image/jpeg")] Jpeg,
    [Description("image/png")] Png,
    [Description("image/gif")] Gif,
    [Description("image/webp")] Webp,
    [Description("image/bmp")] Bmp,
    [Description("image/svg+xml")] Svg,
    [Description("image/tiff")] Tiff,
    [Description("image/ico")] Ico
}