using Application.DTO;
using Domain.Entities;

namespace Application.Utils.Mapper;

public class UserMapper : IMapper<User, UserInfo>
{
    public UserInfo Map(User user)
    {
        return new UserInfo
        {
            Id = user.Id,
            Username = user.Username,
            Status = user.Status
        };
    }
}