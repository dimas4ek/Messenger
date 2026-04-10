using Application.DTO;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Mapper;
using Domain.Entities;

namespace Application.Services;

public class UserService(IUserRepository userRepository, IAppMapper mapper)
{
    public async Task<Result<UserInfo>> GetUserByUsername(string username)
    {
        var user = await userRepository.GetByUsername(username);

        if (user == null) return Result<UserInfo>.Failure(ErrorCode.UserNotFound);

        var userDto = mapper.Map<User, UserInfo>(user);
        return Result<UserInfo>.Success(userDto);
    }

    public async Task<Result<UserInfo>> GetById(int id)
    {
        var user = await userRepository.GetById(id);

        if (user == null) return Result<UserInfo>.Failure(ErrorCode.UserNotFound);

        var userDto = mapper.Map<User, UserInfo>(user);
        return Result<UserInfo>.Success(userDto);
    }
}