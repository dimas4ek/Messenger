using Application.Utils.Mapper;

namespace Application.Utils;

public static class ResultExtensions
{
    public static List<TDest> MapList<TSource, TDest>(
        this IAppMapper mapper,
        IEnumerable<TSource> source)
    {
        return source.Select(mapper.Map<TSource, TDest>).ToList();
    }

    extension(Result result)
    {
        public string ToMessage()
        {
            return ErrorMapper.ToMessage(result.ErrorCode);
        }

        public Result<T> ToFailure<T>()
        {
            return Result<T>.Failure(result.ErrorCode);
        }
    }
}