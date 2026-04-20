using Application.DTO;
using Domain.Entities;

namespace Application.Utils.Mapper;

public class UserMapper(IAppMapper mapper) : IMapper<User, UserInfo>
{
    public UserInfo Map(User user)
    {
        return new UserInfo
        {
            Id = user.Id,
            Username = user.Username,
            Status = user.Status,
            Avatar = user.Avatar != null ? mapper.Map<Image, ImageInfo>(user.Avatar) : null
        };
    }
}