using Application.DTO;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Mapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class ChatService(
    IConversationRepository conversationRepository,
    IMessageRepository messageRepository,
    IUserRepository userRepository,
    IAppMapper mapper)
{
    public async Task<Result<ConversationInfo>> LoadPrivateConversation(int currentUserId, int companionId)
    {
        return await GetOrCreatePrivateConversation(currentUserId, companionId);
    }

    private async Task<Result<ConversationInfo>> GetOrCreatePrivateConversation(int currentUserId, int companionId)
    {
        var conversation = await conversationRepository.GetByUsersId(currentUserId, companionId);

        if (conversation != null)
        {
            var conversationDto = mapper.Map<Conversation, ConversationInfo>(conversation);
            return Result<ConversationInfo>.Success(conversationDto);
        }

        var newConversation = new Conversation
        {
            Type = ConversationType.Private
        };

        await conversationRepository.Add(newConversation);
        await conversationRepository.Save();

        var participants = new[]
        {
            new ConversationParticipant
            {
                ConversationId = newConversation.Id,
                ParticipantId = currentUserId,
                Role = ParticipationRole.Member
            },
            new ConversationParticipant
            {
                ConversationId = newConversation.Id,
                ParticipantId = companionId,
                Role = ParticipationRole.Member
            }
        };

        await conversationRepository.AddParticipants(participants);
        await conversationRepository.Save();

        /*var newConversationDto = new ConversationInfo
        {
            Id = newConversation.Id,
            Type = ConversationType.Private,
            Messages = mapper.MapList<Message, MessageInfo>(
                await messageRepository.GetConversationMessages(newConversation.Id)),
            Participants = mapper.MapList<ConversationParticipant, ParticipantInfo>(
                    await conversationRepository.GetParticipants(newConversation.Id)),
            CreatedAt = newConversation.CreatedAt,
            UpdatedAt = newConversation.UpdatedAt
        };*/

        var newConversationDto = mapper.Map<Conversation, ConversationInfo>(newConversation);
        newConversationDto.Messages = mapper.MapList<Message, MessageInfo>(
            await messageRepository.GetConversationMessages(newConversation.Id));
        newConversationDto.Participants = mapper.MapList<ConversationParticipant, ParticipantInfo>(
            await conversationRepository.GetParticipants(newConversation.Id));

        return Result<ConversationInfo>.Success(newConversationDto);
    }

    public async Task<Result<MessageInfo>> SaveMessage(int currentUserId, int companionId, string message)
    {
        var companionUser = await userRepository.GetById(companionId);

        if (companionUser == null)
            return Result<MessageInfo>.Failure(ErrorCode.UserNotFound);

        var conversationResult = await GetOrCreatePrivateConversation(currentUserId, companionUser.Id);
        if (!conversationResult.IsSuccess)
            return Result<MessageInfo>.Failure(conversationResult.ErrorCode);

        var newMessage = new Message
        {
            ConversationId = conversationResult.Value.Id,
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