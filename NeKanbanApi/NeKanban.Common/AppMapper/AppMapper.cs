using AutoMapper;
using JetBrains.Annotations;
using NeKanban.Common.Attributes;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.AppMapper;

[UsedImplicitly]
[Injectable<IAppMapper>]
public class AppMapper : IAppMapper
{
    private readonly IMapper _mapper;
    public IConfigurationProvider ConfigurationProvider => _mapper.ConfigurationProvider;
    public AppMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TDest Map<TDest, TSource>(TSource source) where TDest : class, IMapFrom<TSource, TDest> where TSource : class
    {
        return _mapper.Map<TDest>(source);
    }

    public TDest Map<TSource, TDest>(TSource source, TDest dest) where TDest : class, IMapFrom<TSource, TDest> where TSource : class
    {
        return _mapper.Map(source, dest);
    }

    public List<TDest> Map<TDest, TSource>(List<TSource> source) where TDest : class, IMapFrom<TSource, TDest> where TSource : class
    {
        return _mapper.Map<List<TDest>>(source);
    }
}