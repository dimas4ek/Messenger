using Application.Services;
using Contracts.DTO;
using Contracts.DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
    {
        var result = await authService.LoginUser(request.Username, request.Password);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new AuthResponse
        {
            User = result.Value
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] AuthRequest request)
    {
        var result = await authService.RegisterUser(request.Username, request.Password);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new AuthResponse
        {
            User = result.Value
        });
    }

    [HttpPost("logout")]
    public async Task<ActionResult<LogoutResponse>> Logout([FromBody] LogoutRequest request)
    {
        var result = await authService.LogoutUser(request.UserId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new LogoutResponse
        {
            Success = result.Value
        });
    }
}