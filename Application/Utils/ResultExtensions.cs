using Application.Utils.Mapper;

namespace Application.Utils;

public static class ResultExtensions
{
    public static string ToMessage(this Result result)
    {
        return ErrorMapper.ToMessage(result.ErrorCode);
    }

    public static Result<T> ToFailure<T>(this Result result)
    {
        return Result<T>.Failure(result.ErrorCode);
    }

    public static List<TDest> MapList<TSource, TDest>(
        this IAppMapper mapper,
        IEnumerable<TSource> source)
    {
        return source.Select(mapper.Map<TSource, TDest>).ToList();
    }
}