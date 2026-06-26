using Client.Utils;
using Contracts.DTO.Auth;

namespace Client.Api.Clients;

public class AuthApiClient(HttpClient httpClient) : ApiClientBase(httpClient)
{
    public Task<ApiResult<AuthResponse>> Login(string username, string password)
    {
        return PostAsync<AuthResponse>("api/auth/login", new AuthRequest
        {
            Username = username,
            Password = password
        });
    }

    public Task<ApiResult<AuthResponse>> Register(string username, string password)
    {
        return PostAsync<AuthResponse>("api/auth/register", new AuthRequest
        {
            Username = username,
            Password = password
        });
    }

    public Task<ApiResult<LogoutResponse>> Logout(int userId)
    {
        return PostAsync<LogoutResponse>("api/auth/logout", new LogoutRequest
        {
            UserId = userId
        });
    }
}