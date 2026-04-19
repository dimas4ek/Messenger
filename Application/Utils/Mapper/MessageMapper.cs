using Application.DTO;
using Domain.Entities;

namespace Application.Utils.Mapper;

public class MessageMapper(IAppMapper mapper) : IMapper<Message, MessageInfo>
{
    public MessageInfo Map(Message friendRequest)
    {
        return new MessageInfo
        {
            Id = friendRequest.Id,
            Text = friendRequest.MessageText,
            Sender = mapper.Map<User, UserInfo>(friendRequest.Sender),
            ChatId = friendRequest.ChatId,
            CreatedAt = friendRequest.CreatedAt,
            IsEdited = friendRequest.IsEdited,
            IsRead = friendRequest.IsRead
        };
    }
}