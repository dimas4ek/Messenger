using Application.Services;
using Contracts.DTO;
using Contracts.DTO.Friend;
using Microsoft.AspNetCore.Mvc;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FriendController : ControllerBase
{
    private readonly FriendService _friendService;

    public FriendController(FriendService friendService)
    {
        _friendService = friendService;
    }

    [HttpGet("find")]
    public async Task<ActionResult<bool>> Find([FromQuery] string friendName)
    {
        var result = await _friendService.FindFriend(friendName);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(true);
    }

    [HttpPost("add")]
    public async Task<ActionResult<FriendResponse>> Add([FromBody] AddFriendRequest request)
    {
        var result = await _friendService.AddFriend(request.CurrentUserId, request.Friend);

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
    public async Task<ActionResult<FriendListResponse>> GetFriendList([FromQuery] int currentUserId)
    {
        var result = await _friendService.GetFriendList(currentUserId);

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

    [HttpGet("search")]
    public async Task<ActionResult<FriendListResponse>> SearchFriends([FromQuery] int currentUserId,
        [FromQuery] string text)
    {
        var result = await _friendService.FriendsFromSearch(currentUserId, text);
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
        var result = await _friendService.AlreadyFriends(request.CurrentUserId, request.FriendName);

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