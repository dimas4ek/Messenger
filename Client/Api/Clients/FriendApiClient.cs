using System.Net.Http.Json;
using Application.Utils;
using Client.Utils;
using Contracts.DTO;
using Contracts.DTO.Friend;

namespace Client.Api.Clients;

public class FriendApiClient(HttpClient httpClient)
{
    public async Task<ApiResult<FriendResponse>> Add(int currentUserId, int friendId)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/friend/add", new AddFriendRequest
            {
                UserId = currentUserId,
                FriendId = friendId
            });

            if (response.IsSuccessStatusCode)
            {
                var friendResponse = await response.Content.ReadFromJsonAsync<FriendResponse>();

                return friendResponse == null
                    ? ApiResult<FriendResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<FriendResponse>.Success(friendResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<FriendResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<FriendResponse>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<ApiResult<FriendListResponse>> GetFriendList(int currentUserId)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/friend/list?userId={currentUserId}");

            if (response.IsSuccessStatusCode)
            {
                var friendListResponse = await response.Content.ReadFromJsonAsync<FriendListResponse>();

                return friendListResponse == null
                    ? ApiResult<FriendListResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<FriendListResponse>.Success(friendListResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<FriendListResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<FriendListResponse>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<ApiResult<AlreadyFriendsResponse>> AlreadyFriends(int currentUserId, int friendId)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/friend/already-friends", new AlreadyFriendsRequest
            {
                UserId = currentUserId,
                FriendId = friendId
            });

            if (response.IsSuccessStatusCode)
            {
                var alreadyFriendsResponse = await response.Content.ReadFromJsonAsync<AlreadyFriendsResponse>();

                return alreadyFriendsResponse == null
                    ? ApiResult<AlreadyFriendsResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<AlreadyFriendsResponse>.Success(alreadyFriendsResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<AlreadyFriendsResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<AlreadyFriendsResponse>.Failure(ErrorCode.DatabaseError);
        }
    }
}