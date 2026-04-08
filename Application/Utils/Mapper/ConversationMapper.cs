using Application.DTO;
using Domain.Entities;
using Domain.Enums;

namespace Application.Utils.Mapper;

public class ConversationMapper(IAppMapper mapper) : IMapper<Conversation, ConversationInfo>
{
    public ConversationInfo Map(Conversation conversation)
    {
        return new ConversationInfo
        {
            Id = conversation.Id,
            Type = ConversationType.Private,
            Messages = conversation.Messages
                .Select(mapper.Map<Message, MessageInfo>)
                .ToList(),
            Participants = conversation.Participants
                .Select(mapper.Map<ConversationParticipant, ParticipantInfo>)
                .ToList(),
            CreatedAt = conversation.CreatedAt,
            UpdatedAt = conversation.UpdatedAt
        };
    }
}