using Application.Services;
using Contracts.DTO;
using Contracts.DTO.Friend;
using Contracts.DTO.User;
using HttpServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FriendController(FriendService friendService, IHubContext<FriendHub> hubContext) : ControllerBase
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

    [HttpGet("requests")]
    public async Task<ActionResult<FriendRequestResponse>> GetFriendRequests([FromQuery(Name = "userId")] int userId)
    {
        var result = await friendService.GetFriendRequests(userId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new FriendRequestResponse
        {
            FriendRequests = result.Value
        });
    }

    [HttpPost("requests/{requestId:int}/accept")]
    public async Task<ActionResult<FriendRequestActionResponse>> AcceptFriendRequest(int requestId)
    {
        var result = await friendService.AcceptFriendRequest(requestId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        var friendRequest = result.Value;

        await hubContext.Clients.Group($"user:{friendRequest.Sender.Id}")
            .SendAsync("FriendAdded", new UserResponse { User = friendRequest.Receiver });

        return Ok(new FriendRequestActionResponse
        {
            AddedFriend = friendRequest.Sender
        });
    }

    [HttpPost("requests/{requestId:int}/decline")]
    public async Task<ActionResult> DeclineFriendRequest(int requestId)
    {
        var result = await friendService.DeclineFriendRequest(requestId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok();
    }
}