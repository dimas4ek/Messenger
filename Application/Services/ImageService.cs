using Application.DTO;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Mapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class ImageService(IImageRepository imageRepository, IAppMapper mapper)
{
    public async Task<Result<ImageInfo>> AddImage(string imageName, byte[] bytes, ImageContentType contentType)
    {
        var image = new Image
        {
            Name = imageName,
            Data = bytes,
            ContentType = contentType
        };

        var imageInfo = await imageRepository.Add(image);
        await imageRepository.Save();

        return Result<ImageInfo>.Success(mapper.Map<Image, ImageInfo>(imageInfo));
    }
}