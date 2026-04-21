using System.Diagnostics;
using System.Net.Http.Json;
using Application.Utils;
using Client.Utils;
using Contracts.DTO;
using Contracts.DTO.Image;
using Contracts.DTO.User;
using Domain.Enums;

namespace Client.Api.Clients;

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

    public async Task<ApiResult<UserResponse>> UpdateUsername(int id, string newUsername)
    {
        try
        {
            var response = await httpClient.PatchAsJsonAsync($"api/user/{id}/username", new UserUpdateRequest
            {
                Username = newUsername
            });

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

    public async Task<ApiResult<UserResponse>> UpdatePassword(int id, string newPassword)
    {
        try
        {
            var response = await httpClient.PatchAsJsonAsync($"api/user/{id}/password", new UserUpdateRequest
            {
                Password = newPassword
            });

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

    public async Task<ApiResult<ImageResponse>> ChangeAvatar(int userId, string name, byte[] imageBytes,
        ImageContentType contentType)
    {
        try
        {
            Debug.WriteLine($"changeavatar name: {name}");
            var response = await httpClient.PatchAsJsonAsync($"api/user/{userId}", new ImageRequest
            {
                Name = name,
                Bytes = imageBytes,
                ContentType = contentType
            });

            if (response.IsSuccessStatusCode)
            {
                var imageResponse = await response.Content.ReadFromJsonAsync<ImageResponse>();

                return imageResponse == null
                    ? ApiResult<ImageResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<ImageResponse>.Success(imageResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<ImageResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<ImageResponse>.Failure(ErrorCode.DatabaseError);
        }
    }
}