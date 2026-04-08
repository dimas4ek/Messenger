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
    public async Task<Result<bool>> FindFriend(string friendName)
    {
        var result = await userService.GetUserByUsername(friendName);
        return result.IsSuccess ? Result<bool>.Success(true) : Result<bool>.Failure(result.ErrorCode);
    }

    public async Task<Result<UserInfo>> AddFriend(int currentUserId, UserInfo friend)
    {
        try
        {
            await friendRepository.AddFriend(currentUserId, friend.Id);

            await friendRepository.Save();

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

    public async Task<Result<List<UserInfo>>> FriendsFromSearch(int currentUserId, string text)
    {
        try
        {
            var friends = await friendRepository.SearchFriends(currentUserId, text);

            return Result<List<UserInfo>>.Success(friends);
        }
        catch
        {
            return Result<List<UserInfo>>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<Result<bool>> AlreadyFriends(int currentUserId, string friendName)
    {
        var result = await userService.GetUserByUsername(friendName);

        return result.IsSuccess
            ? Result<bool>.Success(await friendRepository.IsFriends(currentUserId, result.Value.Id))
            : Result<bool>.Failure(result.ErrorCode);
    }
}