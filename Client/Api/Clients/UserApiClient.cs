using Client.Utils;
using Contracts.DTO.Image;
using Contracts.DTO.User;
using Domain.Enums;

namespace Client.Api.Clients;

public class UserApiClient(HttpClient httpClient) : ApiClientBase(httpClient)
{
    public Task<ApiResult<UserResponse>> Get(string username)
    {
        return GetAsync<UserResponse>($"api/user/get?username={Uri.EscapeDataString(username)}");
    }

    public Task<ApiResult<UserResponse>> UpdateUsername(int id, string newUsername)
    {
        return PatchAsync<UserResponse>($"api/user/{id}/username", new UserUpdateRequest
        {
            Username = newUsername
        });
    }

    public Task<ApiResult<UserResponse>> UpdatePassword(int id, string newPassword)
    {
        return PatchAsync<UserResponse>($"api/user/{id}/password", new UserUpdateRequest
        {
            Password = newPassword
        });
    }

    public Task<ApiResult<UserResponse>> ChangeAvatar(int userId, string name, byte[] imageBytes,
        ImageContentType contentType)
    {
        return PatchAsync<UserResponse>($"api/user/{userId}/avatar", new ImageRequest
        {
            Name = name,
            Bytes = imageBytes,
            ContentType = contentType
        });
    }
}