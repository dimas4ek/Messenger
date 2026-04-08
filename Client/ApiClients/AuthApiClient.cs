using System.Diagnostics;
using System.Net.Http.Json;
using Application.Utils;
using Client.Utils;
using Contracts.DTO;
using Contracts.DTO.Auth;

namespace Client.ApiClients;

public class AuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResult<AuthResponse>> Login(string username, string password)
    {
        return await SendAuthRequest("api/auth/login", username, password);
    }

    public async Task<ApiResult<AuthResponse>> Register(string username, string password)
    {
        return await SendAuthRequest("api/auth/register", username, password);
    }

    public async Task<ApiResult<LogoutResponse>> Logout(int userId)
    {
        try
        {
            var response =
                await _httpClient.PostAsJsonAsync("api/auth/logout", new LogoutRequest
                {
                    UserId = userId
                });

            if (response.IsSuccessStatusCode)
            {
                var logoutResponse = await response.Content.ReadFromJsonAsync<LogoutResponse>();

                return logoutResponse == null
                    ? ApiResult<LogoutResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<LogoutResponse>.Success(logoutResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            return ApiResult<LogoutResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            throw;
            //return ApiResult<bool>.Failure(ErrorCode.AuthError);
        }
    }

    private async Task<ApiResult<AuthResponse>> SendAuthRequest(string url, string username, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(url, new AuthRequest
            {
                Username = username,
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

                return authResponse == null
                    ? ApiResult<AuthResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<AuthResponse>.Success(authResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            return ApiResult<AuthResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch (Exception e)
        {
            return ApiResult<AuthResponse>.Failure(ErrorCode.AuthError);
        }
    }


}