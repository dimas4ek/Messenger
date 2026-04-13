using Application.Utils;

namespace Client.Utils;

public static class ApiResultExtensions
{
    extension<T>(ApiResult<T> result)
    {
        public string ToMessage() => ErrorMapper.ToMessage(result.ErrorCode);
    }
}