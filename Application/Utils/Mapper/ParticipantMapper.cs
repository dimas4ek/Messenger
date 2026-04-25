using Application.DTO;
using Domain.Entities;

namespace Application.Utils.Mapper;

public class ParticipantMapper(IAppMapper mapper) : IMapper<ChatParticipant, ChatParticipantInfo>
{
    public ChatParticipantInfo Map(ChatParticipant participant)
    {
        return new ChatParticipantInfo
        {
            ChatId = participant.ChatId,
            UserId = participant.ParticipantId,
            Role = participant.Role,
            JoinedAt = participant.JoinedAt,
            User = mapper.Map<User, UserInfo>(participant.Participant)
        };
    }
}