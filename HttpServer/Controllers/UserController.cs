using Application.Services;
using Contracts.DTO;
using Contracts.DTO.Friend.Event;
using Contracts.DTO.Image;
using Contracts.DTO.User;
using HttpServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(UserService userService, FriendService friendService, IHubContext<FriendHub> hubContext)
    : ControllerBase
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

        await NotifyFriends(id, new FriendUpdatedEvent { User = result.Value, UsernameChanged = true });

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

    [HttpPatch("{id:int}/avatar")]
    public async Task<ActionResult<UserResponse>> ChangeImage(int id, [FromBody] ImageRequest request)
    {
        var result = await userService.ChangeImage(id, request.Name, request.Bytes, request.ContentType);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        await NotifyFriends(id, new FriendUpdatedEvent { User = result.Value, AvatarChanged = true });

        return Ok(new UserResponse
        {
            User = result.Value
        });
    }

    private async Task NotifyFriends(int userId, FriendUpdatedEvent friendEvent)
    {
        var friendListResult = await friendService.GetFriendList(userId);
        if (!friendListResult.IsSuccess || friendListResult.Value == null) return;

        foreach (var friend in friendListResult.Value)
            await hubContext.Clients.Group($"user:{friend.Id}")
                .SendAsync("FriendUpdated", friendEvent);
    }
}