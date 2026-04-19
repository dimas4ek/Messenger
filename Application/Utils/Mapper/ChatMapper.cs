using Application.DTO;
using Domain.Entities;
using Domain.Enums;

namespace Application.Utils.Mapper;

public class ChatMapper(IAppMapper mapper) : IMapper<Chat, ChatInfo>
{
    public ChatInfo Map(Chat friendRequest)
    {
        return new ChatInfo
        {
            Id = friendRequest.Id,
            Type = ChatType.Private,
            Messages = friendRequest.Messages
                .Select(mapper.Map<Message, MessageInfo>)
                .ToList(),
            Participants = friendRequest.Participants
                .Select(mapper.Map<ChatParticipant, ChatParticipantInfo>)
                .ToList(),
            CreatedAt = friendRequest.CreatedAt,
            UpdatedAt = friendRequest.UpdatedAt
        };
    }
}