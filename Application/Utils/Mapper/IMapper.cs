namespace Application.Utils.Mapper;

public interface IMapper<in TSource, out TDest>
{
    TDest Map(TSource friendRequest);
}