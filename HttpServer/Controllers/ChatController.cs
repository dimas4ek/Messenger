using Application.Services;
using Contracts.DTO;
using Contracts.DTO.Chat;
using HttpServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController(ChatService chatService, IHubContext<ChatHub> hubContext) : ControllerBase
{
    [HttpGet("private")]
    public async Task<ActionResult<ChatResponse>> LoadPrivateChat([FromQuery(Name = "userId")] int userId,
        [FromQuery(Name = "companionId")] int companionId)
    {
        var result = await chatService.LoadPrivateChat(userId, companionId);

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
    public async Task<ActionResult<MessageResponse>> EditMessage(int chatId, int messageId, [FromBody] EditMessageRequest request)
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