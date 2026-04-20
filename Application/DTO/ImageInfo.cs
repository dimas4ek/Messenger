using Domain.Enums;

namespace Application.DTO;

public class ImageInfo
{
    public int Id { get; set; }
    public ImageContentType ContentType { get; set; }
    public byte[] Data { get; set; }
}