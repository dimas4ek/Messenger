using Application.DTO;
using Domain.Entities;

namespace Application.Utils.Mapper;

public class ImageMapper : IMapper<Image, ImageInfo>
{
    public ImageInfo Map(Image image)
    {
        return new ImageInfo
        {
            Id = image.Id,
            ContentType = image.ContentType,
            Data = image.Data
        };
    }
}