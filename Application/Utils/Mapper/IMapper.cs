namespace Application.Utils.Mapper;

public interface IMapper<TSource, TDest>
{
    TDest Map(TSource source);
}