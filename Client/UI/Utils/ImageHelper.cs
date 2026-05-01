using Application.DTO;
using Client.Properties;

namespace Client.UI.Utils;

public static class ImageHelper
{
    public static Image GetAvatar(ImageInfo? image)
    {
        if (image?.Data == null) return Resources.DefaultAvatarImage;

        using var ms = new MemoryStream(image.Data);
        return Image.FromStream(ms);
    }
}