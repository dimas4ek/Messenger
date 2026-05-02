using Application.DTO;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Mapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class ChatService(
    FriendService friendService,
    IChatRepository chatRepository,
    IMessageRepository messageRepository,
    IAppMapper mapper)
{
    public async Task<Result<List<ChatInfo>>> GetChatList(int currentUserId)
    {
        try
        {
            var chats = await chatRepository.GetChatsByUserId(currentUserId);

            var chatsDto = chats.Select(mapper.Map<Chat, ChatInfo>).ToList();

            return Result<List<ChatInfo>>.Success(chatsDto);
        }
        catch
        {
            return Result<List<ChatInfo>>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<Result<ChatInfo>> LoadChat(int chatId, int userId)
    {
        var chat = await chatRepository.GetChat(chatId, userId);

        if (chat == null) return Result<ChatInfo>.Failure(ErrorCode.ChatNotFound);

        var chatDto = mapper.Map<Chat, ChatInfo>(chat);
        return Result<ChatInfo>.Success(chatDto);
    }

    public async Task<Result<ChatInfo>> EditChat(int chatId, string newName)
    {
        var chat = await chatRepository.GetChatById(chatId);

        if (chat == null)
            return Result<ChatInfo>.Failure(ErrorCode.ChatNotFound);

        if (chat.Id != chatId)
            return Result<ChatInfo>.Failure(ErrorCode.AccessDenied);

        chat.Name = newName;

        chatRepository.Update(chat);
        await chatRepository.Save();

        return await GetChat(chatId);
    }

    public async Task<Result<bool>> DeleteChat(int chatId, int currentUserId)
    {
        var chat = await chatRepository.GetChat(chatId, currentUserId);

        if (chat == null)
            return Result<bool>.Failure(ErrorCode.ChatNotFound);

        if (chat.Id != chatId)
            return Result<bool>.Failure(ErrorCode.AccessDenied);

        chatRepository.Remove(chat);
        await chatRepository.Save();

        return Result<bool>.Success(true);
    }

    public async Task<Result<ChatInfo>> CreateGroupChat(string name, int? imageId, int creatorId,
        IEnumerable<int> addedUserIds)
    {
        var newChat = new Chat
        {
            Name = name,
            ImageId = imageId,
            Type = ChatType.Group
        };

        await chatRepository.Add(newChat);
        await chatRepository.Save();

        var participants = addedUserIds.Select(id => new ChatParticipant
        {
            ChatId = newChat.Id,
            ParticipantId = id,
            Role = ChatParticipationRole.Member
        }).ToList();

        participants.Add(new ChatParticipant
        {
            ChatId = newChat.Id,
            ParticipantId = creatorId,
            Role = ChatParticipationRole.Admin
        });

        await chatRepository.AddParticipants(participants);
        await chatRepository.Save();

        var newChatDto = mapper.Map<Chat, ChatInfo>(newChat);

        return Result<ChatInfo>.Success(newChatDto);
    }

    public async Task<Result<ChatInfo>> CreatePrivateChat(int currentUserId, int companionId)
    {
        var friendResult = await friendService.AddFriend(currentUserId, companionId);
        if (!friendResult.IsSuccess) return Result<ChatInfo>.Failure(ErrorCode.AccessDenied);

        var newChat = new Chat
        {
            Type = ChatType.Private
        };

        await chatRepository.Add(newChat);
        await chatRepository.Save();

        var participants = new[]
        {
            new ChatParticipant
                { ChatId = newChat.Id, ParticipantId = currentUserId, Role = ChatParticipationRole.Member },
            new ChatParticipant
                { ChatId = newChat.Id, ParticipantId = companionId, Role = ChatParticipationRole.Member }
        };

        await chatRepository.AddParticipants(participants);
        await chatRepository.Save();

        var newChatDto = mapper.Map<Chat, ChatInfo>(newChat);

        return Result<ChatInfo>.Success(newChatDto);
    }

    public async Task<Result<MessageInfo>> SaveMessage(int chatId, int senderId, string message)
    {
        var newMessage = new Message
        {
            ChatId = chatId,
            SenderId = senderId,
            MessageText = message
        };

        await messageRepository.Add(newMessage);
        await messageRepository.Save();

        return await GetMessage(newMessage.Id);
    }

    public async Task<Result<MessageInfo>> EditMessage(int chatId, int messageId, string newText)
    {
        var message = await messageRepository.GetMessageById(messageId);

        if (message == null)
            return Result<MessageInfo>.Failure(ErrorCode.MessageNotFound);

        if (message.ChatId != chatId)
            return Result<MessageInfo>.Failure(ErrorCode.AccessDenied);

        message.MessageText = newText;
        message.IsEdited = true;

        messageRepository.Update(message);
        await messageRepository.Save();

        return await GetMessage(messageId);
    }

    public async Task<Result<bool>> DeleteMessage(int chatId, int messageId)
    {
        var message = await messageRepository.GetMessageById(messageId);

        if (message == null)
            return Result<bool>.Failure(ErrorCode.MessageNotFound);

        if (message.ChatId != chatId)
            return Result<bool>.Failure(ErrorCode.AccessDenied);

        messageRepository.Remove(message);
        await messageRepository.Save();

        return Result<bool>.Success(true);
    }

    private async Task<Result<ChatInfo>> GetChat(int chatId)
    {
        var chat = await chatRepository.GetChatById(chatId);

        if (chat == null) return Result<ChatInfo>.Failure(ErrorCode.MessageNotFound);

        var chatDto = mapper.Map<Chat, ChatInfo>(chat);
        return Result<ChatInfo>.Success(chatDto);
    }

    private async Task<Result<MessageInfo>> GetMessage(int messageId)
    {
        var message = await messageRepository.GetMessageById(messageId);

        if (message == null) return Result<MessageInfo>.Failure(ErrorCode.MessageNotFound);

        var messageDto = mapper.Map<Message, MessageInfo>(message);
        return Result<MessageInfo>.Success(messageDto);
    }
}