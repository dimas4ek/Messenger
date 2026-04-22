using Application.DTO;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Mapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class AuthService(IUserRepository userRepository, IAppMapper mapper)
{
    public async Task<Result<UserInfo>> RegisterUser(string username, string password)
    {
        var validation = ValidateCredentials(username, password);
        if (!validation.IsSuccess) return validation.ToFailure<UserInfo>();

        try
        {
            if (await userRepository.UserExists(username)) return Result<UserInfo>.Failure(ErrorCode.UsernameTaken);

            var user = new User
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await userRepository.Add(user);
            await userRepository.Save();

            var userDto = MapToUserInfo(user);

            return Result<UserInfo>.Success(userDto);
        }
        catch
        {
            return Result<UserInfo>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<Result<UserInfo>> LoginUser(string username, string password)
    {
        var validation = ValidateCredentials(username, password);
        if (!validation.IsSuccess) return validation.ToFailure<UserInfo>();

        try
        {
            var user = await userRepository.GetByUsername(username);

            if (user == null) return Result<UserInfo>.Failure(ErrorCode.UserNotFound);

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return Result<UserInfo>.Failure(ErrorCode.InvalidPassword);

            user.Status = UserStatus.Online;
            await userRepository.Save();

            var userDto = MapToUserInfo(user);

            return Result<UserInfo>.Success(userDto);
        }
        catch
        {
            return Result<UserInfo>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<Result<UserInfo>> LogoutUser(int userId)
    {
        try
        {
            var user = await userRepository.GetById(userId);

            if (user == null) return Result<UserInfo>.Failure(ErrorCode.UserNotFound);

            user.Status = UserStatus.Offline;
            await userRepository.Save();

            var userDto = MapToUserInfo(user);

            return Result<UserInfo>.Success(userDto);
        }
        catch
        {
            return Result<UserInfo>.Failure(ErrorCode.DatabaseError);
        }
    }

    private static Result ValidateCredentials(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result.Failure(ErrorCode.InvalidUsername);

        if (string.IsNullOrWhiteSpace(password))
            return Result.Failure(ErrorCode.ValidationError);

        return Result.Success();
    }

    private UserInfo MapToUserInfo(User user)
    {
        return mapper.Map<User, UserInfo>(user);
    }
}