using Application.Services;
using Contracts.DTO;
using Contracts.DTO.Chat;
using HttpServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(ChatService chatService, IHubContext<ChatHub> hubContext)
    {
        _chatService = chatService;
        _hubContext = hubContext;
    }

    [HttpGet("private")]
    public async Task<ActionResult<ChatResponse>> LoadPrivateChat([FromQuery] int currentUserId,
        [FromQuery] int companionId)
    {
        var result = await _chatService.LoadPrivateConversation(currentUserId, companionId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new ChatResponse
        {
            Conversation = result.Value
        });
    }

    [HttpPost("message")]
    public async Task<ActionResult<MessageResponse>> SendMessage([FromBody] SendMessageRequest request)
    {
        var result = await _chatService.SaveMessage(request.CurrentUserId, request.CompanionId, request.Message);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        await _hubContext.Clients.Group($"user:{request.CompanionId}")
            .SendAsync("ReceiveMessage", new MessageResponse
            {
                Message = result.Value
            });

        return Ok(new MessageResponse
        {
            Message = result.Value
        });
    }

    /*[HttpGet("message")]
    public async Task<ActionResult<MessageResponse>> GetMessage([FromQuery] int messageId)
    {
        var result = await _chatService.GetMessage(messageId);

        if (!result.IsSuccess)
            return BadRequest(new ErrorResponse
            {
                ErrorCode = result.ErrorCode
            });

        return Ok(new MessageResponse
        {
            Message = result.Value
        });
    }*/
}