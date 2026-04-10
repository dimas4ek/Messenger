using Application.DTO;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Mapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class ChatService(
    IChatRepository chatRepository,
    IMessageRepository messageRepository,
    IUserRepository userRepository,
    IAppMapper mapper)
{
    public async Task<Result<ChatInfo>> LoadPrivateChat(int currentUserId, int companionId)
    {
        return await GetOrCreatePrivateChat(currentUserId, companionId);
    }

    private async Task<Result<ChatInfo>> GetOrCreatePrivateChat(int currentUserId, int companionId)
    {
        var chat = await chatRepository.GetByParticipantsId(currentUserId, companionId);

        if (chat != null)
        {
            var chatDto = mapper.Map<Chat, ChatInfo>(chat);
            return Result<ChatInfo>.Success(chatDto);
        }

        var newChat = new Chat
        {
            Type = ChatType.Private
        };

        await chatRepository.Add(newChat);
        await chatRepository.Save();

        var participants = new[]
        {
            new ChatParticipant
            {
                ChatId = newChat.Id,
                ParticipantId = currentUserId,
                Role = ChatParticipationRole.Member
            },
            new ChatParticipant
            {
                ChatId = newChat.Id,
                ParticipantId = companionId,
                Role = ChatParticipationRole.Member
            }
        };

        await chatRepository.AddParticipants(participants);
        await chatRepository.Save();

        var newChatDto = mapper.Map<Chat, ChatInfo>(newChat);

        newChatDto.Messages = mapper.MapList<Message, MessageInfo>(
            await messageRepository.GetChatMessages(newChat.Id));

        newChatDto.Participants = mapper.MapList<ChatParticipant, ChatParticipantInfo>(
            await chatRepository.GetParticipants(newChat.Id));

        return Result<ChatInfo>.Success(newChatDto);
    }

    public async Task<Result<MessageInfo>> SaveMessage(int currentUserId, int companionId, string message)
    {
        var companionUser = await userRepository.GetById(companionId);

        if (companionUser == null)
            return Result<MessageInfo>.Failure(ErrorCode.UserNotFound);

        var chatResult = await GetOrCreatePrivateChat(currentUserId, companionUser.Id);

        if (!chatResult.IsSuccess || chatResult.Value == null)
            return Result<MessageInfo>.Failure(chatResult.ErrorCode);

        var newMessage = new Message
        {
            ChatId = chatResult.Value.Id,
            SenderId = currentUserId,
            MessageText = message
        };

        await messageRepository.Add(newMessage);
        await messageRepository.Save();

        return await GetMessage(newMessage.Id);
    }

    public async Task<Result<MessageInfo>> GetMessage(int messageId)
    {
        var message = await messageRepository.GetMessageById(messageId);

        if (message == null) return Result<MessageInfo>.Failure(ErrorCode.MessageNotFound);

        var messageDto = mapper.Map<Message, MessageInfo>(message);
        return Result<MessageInfo>.Success(messageDto);
    }

    public async Task<bool> CheckSender(int currentUserId, int messageId)
    {
        var senderId = await messageRepository.GetSenderId(messageId);

        return senderId == currentUserId;
    }
}