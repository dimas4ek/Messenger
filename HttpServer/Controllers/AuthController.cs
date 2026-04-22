using Application.Services;
using Contracts.DTO;
using Contracts.DTO.Auth;
using Contracts.DTO.Friend.Event;
using Contracts.DTO.User;
using HttpServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthService authService, FriendService friendService, IHubContext<FriendHub> hubContext)
    : ControllerBase
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

        await NotifyFriends(result.Value.Id, new FriendStatusEvent { User = result.Value, StatusChanged = true });

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
    public async Task<ActionResult<UserResponse>> Logout([FromBody] LogoutRequest request)
    {
        var result = await authService.LogoutUser(request.UserId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        await NotifyFriends(result.Value.Id, new FriendStatusEvent { User = result.Value, StatusChanged = true });


        return Ok(new UserResponse
        {
            User = result.Value
        });
    }

    private async Task NotifyFriends(int userId, FriendStatusEvent friendEvent)
    {
        var friendListResult = await friendService.GetFriendList(userId);
        if (!friendListResult.IsSuccess || friendListResult.Value == null) return;

        foreach (var friend in friendListResult.Value)
            await hubContext.Clients.Group($"user:{friend.Id}")
                .SendAsync("FriendStatus", friendEvent);
    }
}