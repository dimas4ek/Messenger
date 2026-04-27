using Application.Utils;

namespace Client.Utils;

public class ApiResult<T>
{
    public bool IsSuccess { get; private init; }
    public ErrorCode ErrorCode { get; private init; }
    public T? Value { get; private init; }

    public static ApiResult<T> Success(T value)
    {
        return new ApiResult<T>
        {
            IsSuccess = true,
            ErrorCode = ErrorCode.None,
            Value = value
        };
    }

    public static ApiResult<T> Failure(ErrorCode errorCode)
    {
        return new ApiResult<T>
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            Value = default
        };
    }
}