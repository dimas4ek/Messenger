using Application.Services;
using Contracts;
using Contracts.DTO.Event;
using Contracts.DTO.Image;
using Contracts.DTO.User;
using HttpServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(UserService userService, FriendService friendService, IHubContext<FriendHub> hubContext)
    : AppControllerBase
{
    [HttpGet("get")]
    public async Task<ActionResult<UserResponse>> Get([FromQuery(Name = "username")] string username)
    {
        if (!TryGetValue(await userService.GetUserByUsername(username), out var user, out var error))
            return error;

        return Ok(new UserResponse { User = user });
    }

    [HttpPatch("{id:int}/username")]
    public async Task<ActionResult<UserResponse>> UpdateUsername(int id, [FromBody] UserUpdateRequest request)
    {
        if (request.Username == null) return BadRequest("Username is required");

        if (!TryGetValue(await userService.UpdateUsername(id, request.Username), out var user, out var error))
            return error;

        await NotifyFriends(id, new FriendUpdatedEvent
        {
            User = user,
            UsernameChanged = true
        });

        return Ok(new UserResponse { User = user });
    }

    [HttpPatch("{id:int}/password")]
    public async Task<ActionResult<UserResponse>> UpdatePassword(int id, [FromBody] UserUpdateRequest request)
    {
        if (request.Password == null) return BadRequest("Password is required");

        if (!TryGetValue(await userService.UpdatePassword(id, request.Password), out var user, out var error))
            return error;

        return Ok(new UserResponse { User = user });
    }

    [HttpPatch("{id:int}/avatar")]
    public async Task<ActionResult<UserResponse>> ChangeImage(int id, [FromBody] ImageRequest request)
    {
        if (!TryGetValue(await userService.ChangeImage(id, request.Name, request.Bytes, request.ContentType),
                out var user, out var error))
            return error;

        await NotifyFriends(id, new FriendUpdatedEvent
        {
            User = user,
            AvatarChanged = true
        });

        return Ok(new UserResponse { User = user });
    }

    private async Task NotifyFriends(int userId, FriendUpdatedEvent friendEvent)
    {
        var friendListResult = await friendService.GetFriendList(userId);
        if (!friendListResult.IsSuccess || friendListResult.Value == null) return;

        foreach (var friend in friendListResult.Value)
            await hubContext.Clients.Group($"user:{friend.Id}")
                .SendAsync(HubMethods.Friends.FriendUpdated, friendEvent);
    }
}