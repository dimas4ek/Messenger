using System.Net.Http.Json;
using Application.Utils;
using Client.Utils;
using Contracts.DTO;
using Contracts.DTO.Image;
using Domain.Enums;

namespace Client.Api.Clients;

public class UtilsApiClient(HttpClient httpClient)
{
    public async Task<ApiResult<ImageResponse>> AddImage(string name, byte[] imageBytes,
        ImageContentType contentType)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/utils/image", new ImageRequest
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