using Application.Services;
using Contracts.DTO;
using Contracts.DTO.Event;
using Contracts.DTO.Friend;
using HttpServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FriendController(FriendService friendService, ChatService chatService, IHubContext<FriendHub> hubContext)
    : ControllerBase
{
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

    [HttpPost("requests")]
    public async Task<ActionResult<FriendRequestListResponse>> SendFriendRequest([FromBody] SendFriendRequest request)
    {
        var result = await friendService.AddFriendRequest(request.SenderId, request.ReceiverId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new FriendRequestResponse
        {
            FriendRequest = result.Value
        });
    }

    [HttpGet("requests")]
    public async Task<ActionResult<FriendRequestListResponse>> GetFriendRequests(
        [FromQuery(Name = "userId")] int userId)
    {
        var result = await friendService.GetFriendRequests(userId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new FriendRequestListResponse
        {
            FriendRequests = result.Value
        });
    }

    [HttpPost("requests/{requestId:int}/accept")]
    public async Task<ActionResult<FriendRequestActionResponse>> AcceptFriendRequest(int requestId)
    {
        var friendRequestResult = await friendService.AcceptFriendRequest(requestId);

        if (!friendRequestResult.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = friendRequestResult.ErrorCode
            });

        var friendRequest = friendRequestResult.Value;

        var createdChatResult = await chatService.CreatePrivateChat(friendRequest.Receiver.Id, friendRequest.Sender.Id);

        if (!createdChatResult.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = createdChatResult.ErrorCode
            });

        var chatId = createdChatResult.Value.Id;

        var chatForFriendResult = await chatService.LoadChat(chatId, friendRequest.Sender.Id);
        if (!chatForFriendResult.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = chatForFriendResult.ErrorCode
            });

        var chatForMeResult = await chatService.LoadChat(chatId, friendRequest.Receiver.Id);
        if (!chatForMeResult.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = chatForMeResult.ErrorCode
            });

        await hubContext.Clients.Group($"user:{friendRequest.Sender.Id}")
            .SendAsync("FriendAdded",
                new FriendAddedEvent
                {
                    Friend = friendRequest.Receiver, FriendChat = chatForFriendResult.Value
                });

        return Ok(new FriendRequestActionResponse
        {
            Friend = friendRequest.Sender,
            CreatedChat = chatForMeResult.Value
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