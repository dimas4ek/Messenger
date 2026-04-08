using Application.DTO;
using Domain.Entities;

namespace Application.Utils.Mapper;

public class MessageMapper(IAppMapper mapper) : IMapper<Message, MessageInfo>
{
    public MessageInfo Map(Message message)
    {
        return new MessageInfo
        {
            Id = message.Id,
            Text = message.MessageText,
            Sender = mapper.Map<User, UserInfo>(message.Sender),
            CreatedAt = message.CreatedAt,
            IsEdited = message.IsEdited,
            IsRead = message.IsRead
        };
    }
}