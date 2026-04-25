using Application.Services;
using Contracts.DTO;
using Contracts.DTO.Chat;
using Contracts.DTO.Chat.Event;
using HttpServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController(ChatService chatService, IHubContext<ChatHub> hubContext) : ControllerBase
{
    [HttpGet("list")]
    public async Task<ActionResult<ChatListResponse>> GetChatList([FromQuery(Name = "userId")] int userId)
    {
        var result = await chatService.GetChatList(userId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new ChatListResponse
        {
            Chats = result.Value
        });
    }

    [HttpPost("group")]
    public async Task<ActionResult<ChatResponse>> CreateGroupChat([FromBody] GroupChatRequest request)
    {
        var chatResult =
            await chatService.CreateGroupChat(request.Name, request.ImageId, request.CreatorId, request.AddedUserIds);

        if (!chatResult.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = chatResult.ErrorCode
            });

        var groupChatResult = await chatService.LoadChat(chatResult.Value.Id, request.CreatorId);

        if (!groupChatResult.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = chatResult.ErrorCode
            });

        var groupChat = groupChatResult.Value;

        foreach (var addedUserId in request.AddedUserIds)
            await hubContext.Clients.Group($"user:{addedUserId}")
                .SendAsync("GroupChatCreated", new GroupChatCreatedEvent { Chat = groupChat });

        return Ok(new ChatResponse
        {
            Chat = groupChat
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChatResponse>> LoadChat(int id, [FromQuery(Name = "userId")] int userId)
    {
        var result = await chatService.LoadChat(id, userId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new ChatResponse
        {
            Chat = result.Value
        });
    }

    /*[HttpPatch("{id:int}/image")]
    public async Task<ActionResult<ImageResponse>> ChangeImage(int id, [FromBody] ImageRequest request)
    {
        var result = await chatService.ChangeGroupImage(id, request.Name, request.Bytes, request.ContentType);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new UserResponse
        {
            User = result.Value
        });
    }*/

    [HttpPost("{chatId:int}/messages")]
    public async Task<ActionResult<MessageResponse>> SendMessage(int chatId, [FromBody] SendMessageRequest request)
    {
        var result = await chatService.SaveMessage(chatId, request.SenderId, request.Message);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        await hubContext.Clients.Group($"chat:{chatId}")
            .SendAsync("ReceiveMessage", new MessageResponse
            {
                Message = result.Value
            });

        return Ok(new MessageResponse
        {
            Message = result.Value
        });
    }

    [HttpPatch("{chatId:int}/messages/{messageId:int}")]
    public async Task<ActionResult<MessageResponse>> EditMessage(int chatId, int messageId,
        [FromBody] EditMessageRequest request)
    {
        var result = await chatService.EditMessage(chatId, messageId, request.Message);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        await hubContext.Clients.Group($"chat:{chatId}")
            .SendAsync("EditMessage", new MessageResponse
            {
                Message = result.Value
            });

        return Ok(new MessageResponse
        {
            Message = result.Value
        });
    }

    [HttpDelete("{chatId:int}/messages/{messageId:int}")]
    public async Task<ActionResult<DeleteResponse>> DeleteMessage(int chatId, int messageId)
    {
        var result = await chatService.DeleteMessage(chatId, messageId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        await hubContext.Clients.Group($"chat:{chatId}")
            .SendAsync("DeleteMessage", messageId);

        return Ok(new DeleteResponse
        {
            Success = result.Value
        });
    }
}