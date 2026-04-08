namespace Application.Utils;

public class Result
{
    protected Result(bool isSuccess, ErrorCode errorCode)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
    }

    public bool IsSuccess { get; }
    public ErrorCode ErrorCode { get; }

    public static Result Success()
    {
        return new Result(true, ErrorCode.None);
    }

    public static Result Failure(ErrorCode error)
    {
        return new Result(false, error);
    }
}

public class Result<T> : Result
{
    private Result(bool isSuccess, T value, ErrorCode error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public T Value { get; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, ErrorCode.None);
    }

    public new static Result<T> Failure(ErrorCode error)
    {
        return new Result<T>(false, default!, error);
    }
}