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

    [HttpPatch("{id:int}/username")]
    public async Task<ActionResult<UserResponse>> UpdateUsername(int id, [FromBody] UserUpdateRequest request)
    {
        if (request.Username == null) return BadRequest("Username is required");

        var result = await userService.UpdateUsername(id, request.Username);

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

    [HttpPatch("{id:int}/password")]
    public async Task<ActionResult<UserResponse>> UpdatePassword(int id, [FromBody] UserUpdateRequest request)
    {
        if (request.Password == null) return BadRequest("Password is required");

        var result = await userService.UpdatePassword(id, request.Password);

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