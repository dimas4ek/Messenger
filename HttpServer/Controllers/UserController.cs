using Application.Services;
using Contracts.DTO;
using Contracts.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(UserService userService) : ControllerBase
{
    [HttpGet("get")]
    public async Task<ActionResult<UserResponse>> Get([FromQuery(Name = "username")] string username)
    {
        var result = await userService.GetUserByUsername(username);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new UserResponse
        {
            User = result.Value
        });
    }
}