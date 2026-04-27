using Client.Utils;
using Contracts.DTO.Image;
using Domain.Enums;

namespace Client.Api.Clients;

public class UtilsApiClient(HttpClient httpClient) : ApiClientBase(httpClient)
{
    public Task<ApiResult<ImageResponse>> AddImage(string name, byte[] imageBytes,
        ImageContentType contentType)
    {
        return PostAsync<ImageResponse>("api/utils/image", new ImageRequest
        {
            Name = name,
            Bytes = imageBytes,
            ContentType = contentType
        });
    }
}