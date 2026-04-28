using System.Net.Http.Json;
using Application.Utils;
using Client.Utils;
using Contracts.DTO;
using Contracts.DTO.Friend;

namespace Client.Api.Clients;

public class FriendApiClient(HttpClient httpClient) : ApiClientBase(httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public Task<ApiResult<FriendListResponse>> GetFriendList(int currentUserId)
    {
        return GetAsync<FriendListResponse>($"api/friend/list?userId={currentUserId}");
    }

    public Task<ApiResult<AlreadyFriendsResponse>> AlreadyFriends(int currentUserId, int friendId)
    {
        return PostAsync<AlreadyFriendsResponse>("api/friend/already-friends", new AlreadyFriendsRequest
        {
            UserId = currentUserId,
            FriendId = friendId
        });
    }

    public Task<ApiResult<FriendRequestResponse>> SendFriendRequest(int senderId, int receiverId)
    {
        return PostAsync<FriendRequestResponse>("api/friend/requests", new SendFriendRequest
        {
            SenderId = senderId,
            ReceiverId = receiverId
        });
    }

    public Task<ApiResult<FriendRequestListResponse>> GetFriendRequests(int currentUserId)
    {
        return GetAsync<FriendRequestListResponse>($"api/friend/requests?userId={currentUserId}");
    }

    public async Task<ApiResult<FriendRequestActionResponse>> FriendRequestAction(int requestId, bool acceptRequest)
    {
        try
        {
            var response =
                await _httpClient.PostAsync($"api/friend/requests/{requestId}/{(acceptRequest ? "accept" : "decline")}",
                    null);

            if (response.IsSuccessStatusCode)
            {
                if (!acceptRequest)
                    return ApiResult<FriendRequestActionResponse>.Success(null!);
                var friendRequestActionResponse =
                    await response.Content.ReadFromJsonAsync<FriendRequestActionResponse>();

                return friendRequestActionResponse == null
                    ? ApiResult<FriendRequestActionResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<FriendRequestActionResponse>.Success(friendRequestActionResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<FriendRequestActionResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<FriendRequestActionResponse>.Failure(ErrorCode.DatabaseError);
        }
    }
}