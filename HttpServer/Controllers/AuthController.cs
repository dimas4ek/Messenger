using Application.Services;
using Contracts.DTO;
using Contracts.DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
    {
        var result = await _authService.LoginUser(request.Username, request.Password);

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
        var result = await _authService.RegisterUser(request.Username, request.Password);

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
        var result = await _authService.LogoutUser(request.UserId);

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