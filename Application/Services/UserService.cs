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

        if (user == null)
            return Result<UserInfo>.Failure(ErrorCode.UserNotFound);

        return Result<UserInfo>.Success(
            mapper.Map<User, UserInfo>(user));
    }

    public async Task<Result<UserInfo>> GetById(int id)
    {
        var user = await userRepository.GetById(id);

        return user == null
            ? Result<UserInfo>.Failure(ErrorCode.UserNotFound)
            : Result<UserInfo>.Success(mapper.Map<User, UserInfo>(user));
    }

    public async Task<Result<UserInfo>> UpdateUsername(int id, string newUsername)
    {
        if (await userRepository.UserExists(newUsername))
            return Result<UserInfo>.Failure(ErrorCode.UsernameTaken);

        var userResult = await GetUserEntity(id);

        if (!userResult.IsSuccess)
            return Result<UserInfo>.Failure(userResult.ErrorCode);

        return await UpdateUserEntity(
            userResult.Value!,
            u => u.Username = newUsername);
    }

    public async Task<Result<UserInfo>> UpdatePassword(int id, string newPassword)
    {
        var userResult = await GetUserEntity(id);

        if (!userResult.IsSuccess)
            return Result<UserInfo>.Failure(userResult.ErrorCode);

        return await UpdateUserEntity(
            userResult.Value!,
            u => u.Password = BCrypt.Net.BCrypt.HashPassword(newPassword));
    }

    private async Task<Result<UserInfo>> UpdateUserEntity(
        User user,
        Action<User> updateAction)
    {
        updateAction(user);

        userRepository.Update(user);
        await userRepository.Save();

        return Result<UserInfo>.Success(mapper.Map<User, UserInfo>(user));
    }

    private async Task<Result<User>> GetUserEntity(int id)
    {
        var user = await userRepository.GetById(id);

        return user == null
            ? Result<User>.Failure(ErrorCode.UserNotFound)
            : Result<User>.Success(user);
    }
}