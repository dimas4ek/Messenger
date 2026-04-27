using System.Net.Http.Json;
using Application.Utils;
using Client.Utils;
using Contracts.DTO;

namespace Client.Api.Clients;

public abstract class ApiClientBase(HttpClient httpClient)
{
    protected Task<ApiResult<TResponse>> GetAsync<TResponse>(string url)
    {
        return SendAsync<TResponse>(() => httpClient.GetAsync(url));
    }

    protected Task<ApiResult<TResponse>> PostAsync<TResponse>(string url, object body)
    {
        return SendAsync<TResponse>(() => httpClient.PostAsJsonAsync(url, body));
    }

    protected Task<ApiResult<TResponse>> PatchAsync<TResponse>(string url, object body)
    {
        return SendAsync<TResponse>(() => httpClient.PatchAsJsonAsync(url, body));
    }

    protected Task<ApiResult<TResponse>> PutAsync<TResponse>(string url, object body)
    {
        return SendAsync<TResponse>(() => httpClient.PutAsJsonAsync(url, body));
    }

    protected Task<ApiResult<TResponse>> DeleteAsync<TResponse>(string url)
    {
        return SendAsync<TResponse>(() => httpClient.DeleteAsync(url));
    }

    private async Task<ApiResult<T>> SendAsync<T>(Func<Task<HttpResponseMessage>> request)
    {
        try
        {
            var response = await request();

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();

                return result == null
                    ? ApiResult<T>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<T>.Success(result);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<T>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<T>.Failure(ErrorCode.DatabaseError);
        }
    }
}