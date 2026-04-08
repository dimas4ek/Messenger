namespace Application.Utils.Mapper;

public interface IAppMapper
{
    TDest Map<TSource, TDest>(TSource source);
}