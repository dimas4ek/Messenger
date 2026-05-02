using Application.DTO;
using Application.Services;
using Contracts;
using Contracts.DTO;
using Contracts.DTO.Chat;
using Contracts.DTO.Event;
using HttpServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController(
    ChatService chatService,
    IHubContext<ChatHub> chatHubContext) : AppControllerBase
{
    [HttpGet("list")]
    public async Task<ActionResult<ChatListResponse>> GetChatList([FromQuery(Name = "userId")] int userId)
    {
        if (!TryGetValue(await chatService.GetChatList(userId), out var chatList, out var error))
            return error;

        return Ok(new ChatListResponse { Chats = chatList });
    }

    [HttpPost("group")]
    public async Task<ActionResult<ChatResponse>> CreateGroupChat([FromBody] GroupChatRequest request)
    {
        if (!TryGetValue(await chatService.CreateGroupChat(request.Name, request.ImageId, request.CreatorId,
                    request.AddedUserIds),
                out var chat,
                out var error) ||
            !TryGetValue(await chatService.LoadChat(chat.Id, request.CreatorId),
                out var groupChat,
                out error))
            return error;

        foreach (var addedUserId in request.AddedUserIds)
            await chatHubContext.Clients.Group($"user:{addedUserId}")
                .SendAsync(HubMethods.Chats.GroupChatCreated, new GroupChatCreatedEvent { Chat = groupChat });

        return Ok(new ChatResponse { Chat = groupChat });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChatResponse>> LoadChat(int id, [FromQuery(Name = "userId")] int userId)
    {
        if (!TryGetValue(await chatService.LoadChat(id, userId), out var chat, out var error))
            return error;

        return Ok(new ChatResponse { Chat = chat });
    }

    [HttpPatch("{chatId:int}")]
    public async Task<ActionResult<ChatResponse>> EditChat(int chatId, [FromBody] EditChatRequest request)
    {
        if (!TryGetValue(await chatService.EditChat(chatId, request.Name), out var chat,
                out var error))
            return error;

        var chatResponse = new ChatResponse { Chat = chat };

        await NotifyChatParticipants(chat, new GroupChatUpdatedEvent { Chat = chat });

        return Ok(chatResponse);
    }

    [HttpDelete("private/{chatId:int}")]
    public async Task<ActionResult<DeleteResponse>> DeleteChat(int chatId,
        [FromQuery(Name = "currentUserId")] int currentUserId, [FromQuery(Name = "friendId")] int friendId)
    {
        if (!TryGetValue(await chatService.DeleteChat(chatId, currentUserId), out var success, out var error))
            return error;

        await chatHubContext.Clients.Group($"user:{friendId}")
            .SendAsync(HubMethods.Chats.GroupChatDeleted, chatId);

        return Ok(new DeleteResponse { Success = success });
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
        if (!TryGetValue(await chatService.SaveMessage(chatId, request.SenderId, request.Text), out var message,
                out var error))
            return error;

        var messageResponse = new MessageResponse { Message = message };

        await chatHubContext.Clients.Group($"chat:{chatId}")
            .SendAsync(HubMethods.Messages.MessageReceived, messageResponse);

        return Ok(messageResponse);
    }

    [HttpPatch("{chatId:int}/messages/{messageId:int}")]
    public async Task<ActionResult<MessageResponse>> EditMessage(int chatId, int messageId,
        [FromBody] EditMessageRequest request)
    {
        if (!TryGetValue(await chatService.EditMessage(chatId, messageId, request.Message), out var message,
                out var error))
            return error;

        var messageResponse = new MessageResponse { Message = message };

        await chatHubContext.Clients.Group($"chat:{chatId}")
            .SendAsync(HubMethods.Messages.MessageUpdated, messageResponse);

        return Ok(messageResponse);
    }

    [HttpDelete("{chatId:int}/messages/{messageId:int}")]
    public async Task<ActionResult<DeleteResponse>> DeleteMessage(int chatId, int messageId)
    {
        if (!TryGetValue(await chatService.DeleteMessage(chatId, messageId), out var success, out var error))
            return error;

        await chatHubContext.Clients.Group($"chat:{chatId}")
            .SendAsync(HubMethods.Messages.MessageDeleted, messageId);

        return Ok(new DeleteResponse { Success = success });
    }

    private async Task NotifyChatParticipants(ChatInfo chat, GroupChatUpdatedEvent groupChatEvent)
    {
        foreach (var participant in chat.Participants)
            await chatHubContext.Clients.Group($"user:{participant.UserId}")
                .SendAsync(HubMethods.Chats.GroupChatUpdated, groupChatEvent);
    }
}