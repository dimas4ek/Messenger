using Application.Services;
using Contracts.DTO;
using Contracts.DTO.Friend;
using Microsoft.AspNetCore.Mvc;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FriendController(FriendService friendService) : ControllerBase
{
    [HttpPost("add")]
    public async Task<ActionResult<FriendResponse>> Add([FromBody] AddFriendRequest request)
    {
        var result = await friendService.AddFriend(request.UserId, request.FriendId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new FriendResponse
        {
            Friend = result.Value
        });
    }

    [HttpGet("list")]
    public async Task<ActionResult<FriendListResponse>> GetFriendList([FromQuery(Name = "userId")] int userId)
    {
        var result = await friendService.GetFriendList(userId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new FriendListResponse
        {
            Friends = result.Value
        });
    }

    [HttpPost("already-friends")]
    public async Task<ActionResult<AlreadyFriendsResponse>> AlreadyFriends([FromBody] AlreadyFriendsRequest request)
    {
        var result = await friendService.AlreadyFriends(request.UserId, request.FriendId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new AlreadyFriendsResponse
        {
            IsFriends = result.Value
        });
    }
}