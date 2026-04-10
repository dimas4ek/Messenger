using System.Net.Http.Json;
using Application.Utils;
using Client.Utils;
using Contracts.DTO;
using Contracts.DTO.User;

namespace Client.ApiClients;

public class UserApiClient(HttpClient httpClient)
{
    public async Task<ApiResult<UserResponse>> Get(string username)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/user/get?username={Uri.EscapeDataString(username)}");

            if (response.IsSuccessStatusCode)
            {
                var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();

                return userResponse == null
                    ? ApiResult<UserResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<UserResponse>.Success(userResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<UserResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<UserResponse>.Failure(ErrorCode.DatabaseError);
        }
    }
}