using Application.DTO;
using Domain.Entities;

namespace Application.Utils.Mapper;

public class FriendRequestMapper(IAppMapper mapper) : IMapper<FriendRequest, FriendRequestInfo>
{
    public FriendRequestInfo Map(FriendRequest friendRequest)
    {
        return new FriendRequestInfo
        {
            Id = friendRequest.Id,
            Sender = mapper.Map<User, UserInfo>(friendRequest.Sender),
            Receiver = mapper.Map<User, UserInfo>(friendRequest.Receiver),
            CreatedAt = friendRequest.CreatedAt
        };
    }
}