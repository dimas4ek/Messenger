using System.Net.Http.Json;
using Application.DTO;
using Application.Utils;
using Client.Utils;
using Contracts.DTO;
using Contracts.DTO.Friend;

namespace Client.ApiClients;

public class FriendApiClient
{
    private readonly HttpClient _httpClient;

    public FriendApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResult<bool>> Find(string friendName)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"api/friend/find?friendName={Uri.EscapeDataString(friendName)}");

            if (response.IsSuccessStatusCode)
            {
                var value = await response.Content.ReadFromJsonAsync<bool>();
                return ApiResult<bool>.Success(value);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<bool>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<bool>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<ApiResult<FriendResponse>> Add(int currentUserId, UserInfo friend)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/friend/add", new AddFriendRequest
            {
                CurrentUserId = currentUserId,
                Friend = friend
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
            var response = await _httpClient.GetAsync($"api/friend/list?currentUserId={currentUserId}");

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

    public async Task<ApiResult<FriendListResponse>> FriendsFromSearch(int currentUserId, string text)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"api/friend/search?currentUserId={currentUserId}&text={Uri.EscapeDataString(text)}");

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

    public async Task<ApiResult<AlreadyFriendsResponse>> AlreadyFriends(int currentUserId, string friendName)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"api/friend/already-friends?currentUserId={currentUserId}&friendName={Uri.EscapeDataString(friendName)}");

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