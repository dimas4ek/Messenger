using Application.DTO;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Mapper;
using Domain.Entities;

namespace Application.Services;

public class FriendService(
    IFriendRepository friendRepository,
    IFriendRequestRepository friendRequestRepository,
    UserService userService,
    IAppMapper mapper)
{
    public async Task<Result<UserInfo>> AddFriend(int userId, int friendId)
    {
        try
        {
            await friendRepository.AddFriend(userId, friendId);

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

    public async Task<Result<FriendRequestInfo>> AddFriendRequest(int senderId, int receiverId)
    {
        try
        {
            var newRequest = new FriendRequest
            {
                SenderId = senderId,
                ReceiverId = receiverId
            };

            var requestInfo = await friendRequestRepository.Add(newRequest);
            await friendRequestRepository.Save();

            return Result<FriendRequestInfo>.Success(mapper.Map<FriendRequest, FriendRequestInfo>(requestInfo));
        }
        catch
        {
            return Result<FriendRequestInfo>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<Result<List<FriendRequestInfo>>> GetFriendRequests(int currentUserId)
    {
        try
        {
            var result = await friendRequestRepository.GetRequestsByUserId(currentUserId);

            var friendRequests = result
                .Select(mapper.Map<FriendRequest, FriendRequestInfo>)
                .ToList();

            return Result<List<FriendRequestInfo>>.Success(friendRequests);
        }
        catch
        {
            return Result<List<FriendRequestInfo>>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<Result<FriendRequestInfo>> AcceptFriendRequest(int requestId)
    {
        try
        {
            var request = await friendRequestRepository.GetByIdWithUsers(requestId);
            if (request == null) return Result<FriendRequestInfo>.Failure(ErrorCode.FriendRequestNotFound);

            friendRequestRepository.Remove(request);
            await friendRequestRepository.Save();

            return Result<FriendRequestInfo>.Success(mapper.Map<FriendRequest, FriendRequestInfo>(request));
        }
        catch
        {
            return Result<FriendRequestInfo>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<Result<bool>> DeclineFriendRequest(int requestId)
    {
        try
        {
            var request = await friendRequestRepository.GetById(requestId);
            if (request == null) return Result<bool>.Failure(ErrorCode.FriendRequestNotFound);

            friendRequestRepository.Remove(request);
            await friendRequestRepository.Save();

            return Result<bool>.Success(true);
        }
        catch
        {
            return Result<bool>.Failure(ErrorCode.DatabaseError);
        }
    }
}