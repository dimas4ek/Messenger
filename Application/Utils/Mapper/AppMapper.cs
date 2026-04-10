using Microsoft.Extensions.DependencyInjection;

namespace Application.Utils.Mapper;

public class AppMapper(IServiceProvider serviceProvider) : IAppMapper
{
    public TDest Map<TSource, TDest>(TSource source)
    {
        if (source == null) return default;

        var mapper = serviceProvider.GetService<IMapper<TSource, TDest>>();

        return mapper == null
            ? throw new InvalidOperationException(
                $"Mapper from {typeof(TSource).Name} to {typeof(TDest).Name} not found")
            : mapper.Map(source);
    }
}