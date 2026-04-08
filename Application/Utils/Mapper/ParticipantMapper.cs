using Application.DTO;
using Domain.Entities;

namespace Application.Utils.Mapper;

public class ParticipantMapper : IMapper<ConversationParticipant, ParticipantInfo>
{
    public ParticipantInfo Map(ConversationParticipant conversationParticipant)
    {
        return new ParticipantInfo
        {
            ConversationId = conversationParticipant.ConversationId,
            UserId = conversationParticipant.ParticipantId,
            Role = conversationParticipant.Role,
            JoinedAt = conversationParticipant.JoinedAt
        };
    }
}