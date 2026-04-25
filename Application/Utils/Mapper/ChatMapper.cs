using Application.DTO;
using Domain.Entities;

namespace Application.Utils.Mapper;

public class ChatMapper(IAppMapper mapper) : IMapper<Chat, ChatInfo>
{
    public ChatInfo Map(Chat chat)
    {
        return new ChatInfo
        {
            Id = chat.Id,
            Type = chat.Type,
            Name = chat.Name,
            Image = chat.Image != null ? mapper.Map<Image, ImageInfo>(chat.Image) : null,
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