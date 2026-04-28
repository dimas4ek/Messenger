using Application.Services;
using Contracts.DTO.Auth;
using Contracts.DTO.Event;
using HttpServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthService authService, FriendService friendService, IHubContext<FriendHub> hubContext)
    : AppControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
    {
        if (!TryGetValue(await authService.LoginUser(request.Username, request.Password), out var user, out var error))
            return error;

        await NotifyFriends(user.Id, new FriendStatusEvent
        {
            User = user,
            StatusChanged = true
        });

        return Ok(new AuthResponse { User = user });
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] AuthRequest request)
    {
        if (!TryGetValue(await authService.LoginUser(request.Username, request.Password), out var user, out var error))
            return error;

        return Ok(new AuthResponse { User = user });
    }

    [HttpPost("logout")]
    public async Task<ActionResult<LogoutResponse>> Logout([FromBody] LogoutRequest request)
    {
        if (!TryGetValue(await authService.LogoutUser(request.UserId), out var user, out var error))
            return error;

        await NotifyFriends(user.Id, new FriendStatusEvent
        {
            User = user,
            StatusChanged = true
        });

        return Ok(new LogoutResponse { Success = true });
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