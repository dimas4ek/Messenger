using Application.Utils;

namespace Client.Utils;

public static class ApiResultExtensions
{
    public static string ToMessage<T>(this ApiResult<T> result)
    {
        return ErrorMapper.ToMessage(result.ErrorCode);
    }
}