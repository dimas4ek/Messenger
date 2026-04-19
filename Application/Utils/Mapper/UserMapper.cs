using Application.DTO;
using Domain.Entities;

namespace Application.Utils.Mapper;

public class UserMapper : IMapper<User, UserInfo>
{
    public UserInfo Map(User friendRequest)
    {
        return new UserInfo
        {
            Id = friendRequest.Id,
            Username = friendRequest.Username,
            Status = friendRequest.Status
        };
    }
}