using Application.DTO;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Mapper;
using Domain.Entities;

namespace Application.Services;

public class FriendService(
    IFriendRepository friendRepository,
    UserService userService,
    IAppMapper mapper)
{
    public async Task<Result<UserInfo>> AddFriend(int currentUserId, int friendId)
    {
        try
        {
            await friendRepository.AddFriend(currentUserId, friendId);

            await friendRepository.Save();

            var friendResult = await userService.GetById(friendId);

            if (!friendResult.IsSuccess || friendResult.Value == null)
                return Result<UserInfo>.Failure(friendResult.ErrorCode);
            var friend = friendResult.Value;

            return Result<UserInfo>.Success(friend);
        }
        catch
        {
            return Result<UserInfo>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<Result<List<UserInfo>>> GetFriendList(int currentUserId)
    {
        try
        {
            var friends = await friendRepository.GetFriendsByUserId(currentUserId);

            var friendsDto = friends.Select(f => mapper.Map<User, UserInfo>(f.Friend)).ToList();

            return Result<List<UserInfo>>.Success(friendsDto);
        }
        catch
        {
            return Result<List<UserInfo>>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<Result<bool>> AlreadyFriends(int currentUserId, int friendId)
    {
        var result = await userService.GetById(friendId);

        if (!result.IsSuccess || result.Value == null) return Result<bool>.Failure(ErrorCode.UserNotFound);

        return result.IsSuccess
            ? Result<bool>.Success(await friendRepository.IsFriends(currentUserId, friendId))
            : Result<bool>.Failure(result.ErrorCode);
    }
}