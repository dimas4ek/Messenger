using Application.Services;
using Contracts.DTO;
using Contracts.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("get")]
    public async Task<ActionResult<UserResponse>> Get([FromQuery] string username)
    {
        var result = await _userService.GetUserByUsername(username);

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