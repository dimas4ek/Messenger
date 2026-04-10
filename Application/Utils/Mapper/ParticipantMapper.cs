using Application.DTO;
using Domain.Entities;

namespace Application.Utils.Mapper;

public class ParticipantMapper : IMapper<ChatParticipant, ChatParticipantInfo>
{
    public ChatParticipantInfo Map(ChatParticipant chatParticipant)
    {
        return new ChatParticipantInfo
        {
            ChatId = chatParticipant.ChatId,
            UserId = chatParticipant.ParticipantId,
            Role = chatParticipant.Role,
            JoinedAt = chatParticipant.JoinedAt
        };
    }
}