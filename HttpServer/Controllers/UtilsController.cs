using Application.Services;
using Contracts.DTO;
using Contracts.DTO.Image;
using Microsoft.AspNetCore.Mvc;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UtilsController(ImageService imageService) : ControllerBase
{
    [HttpPost("image")]
    public async Task<ActionResult<ImageResponse>> AddImage([FromBody] ImageRequest request)
    {
        var result = await imageService.AddImage(request.Name, request.Bytes, request.ContentType);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new ImageResponse
        {
            Image = result.Value
        });
    }
}