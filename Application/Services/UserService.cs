using System.Diagnostics;
using Application.DTO;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Mapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class UserService(IUserRepository userRepository, IImageRepository imageRepository, IAppMapper mapper)
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

    public async Task<Result<ImageInfo>> ChangeImage(int userId, string imageName, byte[] bytes,
        ImageContentType contentType)
    {
        var userResult = await GetUserEntity(userId);

        if (!userResult.IsSuccess)
            return Result<ImageInfo>.Failure(userResult.ErrorCode);

        var image = new Image
        {
            Name = imageName,
            Data = bytes,
            ContentType = contentType
        };

        Debug.WriteLine("image");
        Debug.WriteLine(image.Name);
        Debug.WriteLine(image.ContentType);

        var imageInfo = await imageRepository.Add(image);
        await imageRepository.Save();

        Debug.WriteLine("imageinfo");
        Debug.WriteLine(imageInfo.Name);
        Debug.WriteLine(imageInfo.ContentType);

        await UpdateUserEntity(userResult.Value!, u => u.Avatar = imageInfo);

        return Result<ImageInfo>.Success(mapper.Map<Image, ImageInfo>(imageInfo));
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