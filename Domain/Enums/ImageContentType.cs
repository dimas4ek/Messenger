using System.ComponentModel;
using NpgsqlTypes;

namespace Domain.Enums;

public enum ImageContentType
{
    [Description("image/jpeg")] [PgName("image/jpeg")]
    Jpeg,

    [Description("image/png")] [PgName("image/png")]
    Png,

    [Description("image/gif")] [PgName("image/gif")]
    Gif,

    [Description("image/webp")] [PgName("image/webp")]
    Webp,

    [Description("image/bmp")] [PgName("image/bmp")]
    Bmp,

    [Description("image/svg+xml")] [PgName("image/svg+xml")]
    Svg,

    [Description("image/tiff")] [PgName("image/tiff")]
    Tiff,

    [Description("image/ico")] [PgName("image/ico")]
    Ico
}