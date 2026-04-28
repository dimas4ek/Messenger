using Application.Services;
using Contracts.DTO.Image;
using Microsoft.AspNetCore.Mvc;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UtilsController(ImageService imageService) : AppControllerBase
{
    [HttpPost("image")]
    public async Task<ActionResult<ImageResponse>> AddImage([FromBody] ImageRequest request)
    {
        if (!TryGetValue(await imageService.AddImage(request.Name, request.Bytes, request.ContentType), out var image,
                out var error))
            return error;

        return Ok(new ImageResponse { Image = image });
    }
}