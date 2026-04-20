using Application.DTO;
using Domain.Entities;
using Domain.Enums;

namespace Application.Utils.Mapper;

public class ChatMapper(IAppMapper mapper) : IMapper<Chat, ChatInfo>
{
    public ChatInfo Map(Chat chat)
    {
        return new ChatInfo
        {
            Id = chat.Id,
            Type = ChatType.Private,
            Messages = chat.Messages
                .Select(mapper.Map<Message, MessageInfo>)
                .ToList(),
            Participants = chat.Participants
                .Select(mapper.Map<ChatParticipant, ChatParticipantInfo>)
                .ToList(),
            CreatedAt = chat.CreatedAt,
            UpdatedAt = chat.UpdatedAt
        };
    }
}