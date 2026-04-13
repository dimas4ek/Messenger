using Application.Utils;

namespace Client.Utils;

public static class ApiResultExtensions
{
    extension<T>(ApiResult<T> result)
    {
        public string ToMessage()
        {
            return ErrorMapper.ToMessage(result.ErrorCode);
        }
    }
}