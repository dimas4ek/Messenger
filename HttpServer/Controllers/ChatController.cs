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

    [HttpPost("message")]
    public async Task<ActionResult<MessageResponse>> SendMessage([FromBody] SendMessageRequest request)
    {
        var result = await chatService.SaveMessage(request.SenderId, request.CompanionId, request.Message);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        await hubContext.Clients.Group($"user:{request.CompanionId}")
            .SendAsync("ReceiveMessage", new MessageResponse
            {
                Message = result.Value
            });

        return Ok(new MessageResponse
        {
            Message = result.Value
        });
    }
}