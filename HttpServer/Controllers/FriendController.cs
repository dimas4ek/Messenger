using Application.Services;
using Contracts;
using Contracts.DTO.Event;
using Contracts.DTO.Friend;
using HttpServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FriendController(FriendService friendService, ChatService chatService, IHubContext<FriendHub> hubContext)
    : AppControllerBase
{
    [HttpGet("list")]
    public async Task<ActionResult<FriendListResponse>> GetFriendList([FromQuery(Name = "userId")] int userId)
    {
        if (!TryGetValue(await friendService.GetFriendList(userId), out var friendList, out var error))
            return error;

        return Ok(new FriendListResponse { Friends = friendList });
    }

    [HttpPost("already-friends")]
    public async Task<ActionResult<AlreadyFriendsResponse>> AlreadyFriends([FromBody] AlreadyFriendsRequest request)
    {
        if (!TryGetValue(await friendService.AlreadyFriends(request.UserId, request.FriendId), out var isFriends,
                out var error))
            return error;

        return Ok(new AlreadyFriendsResponse { IsFriends = isFriends });
    }

    [HttpPost("requests")]
    public async Task<ActionResult<FriendRequestResponse>> SendFriendRequest([FromBody] SendFriendRequest request)
    {
        if (!TryGetValue(await friendService.AddFriendRequest(request.SenderId, request.ReceiverId),
                out var friendRequest,
                out var error))
            return error;

        return Ok(new FriendRequestResponse { FriendRequest = friendRequest });
    }

    [HttpGet("requests")]
    public async Task<ActionResult<FriendRequestListResponse>> GetFriendRequests(
        [FromQuery(Name = "userId")] int userId)
    {
        if (!TryGetValue(await friendService.GetFriendRequests(userId),
                out var friendRequestList,
                out var error))
            return error;

        return Ok(new FriendRequestListResponse { FriendRequests = friendRequestList });
    }

    [HttpPost("requests/{requestId:int}/accept")]
    public async Task<ActionResult<FriendRequestActionResponse>> AcceptFriendRequest(int requestId)
    {
        if (!TryGetValue(await friendService.AcceptFriendRequest(requestId),
                out var friendRequest,
                out var error) ||
            !TryGetValue(await chatService.CreatePrivateChat(friendRequest.Receiver.Id, friendRequest.Sender.Id),
                out var createdChat,
                out error) ||
            !TryGetValue(await chatService.LoadChat(createdChat.Id, friendRequest.Sender.Id),
                out var chatForFriend,
                out error) ||
            !TryGetValue(await chatService.LoadChat(createdChat.Id, friendRequest.Receiver.Id),
                out var chatForMe,
                out error))
            return error;

        await hubContext.Clients.Group($"user:{friendRequest.Sender.Id}")
            .SendAsync(HubMethods.Friends.FriendAdded,
                new FriendAddedEvent
                {
                    Friend = friendRequest.Receiver,
                    FriendChat = chatForFriend
                });

        return Ok(new FriendRequestActionResponse
        {
            Friend = friendRequest.Sender,
            CreatedChat = chatForMe
        });
    }

    [HttpPost("requests/{requestId:int}/decline")]
    public async Task<ActionResult> DeclineFriendRequest(int requestId)
    {
        if (!TryGetValue(await friendService.DeclineFriendRequest(requestId), out var error))
            return error;

        return Ok();
    }
}