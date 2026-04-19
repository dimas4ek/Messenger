using Application.DTO;
using Domain.Entities;

namespace Application.Utils.Mapper;

public class ParticipantMapper : IMapper<ChatParticipant, ChatParticipantInfo>
{
    public ChatParticipantInfo Map(ChatParticipant friendRequest)
    {
        return new ChatParticipantInfo
        {
            ChatId = friendRequest.ChatId,
            UserId = friendRequest.ParticipantId,
            Role = friendRequest.Role,
            JoinedAt = friendRequest.JoinedAt
        };
    }
}