using AutoMapper;
using Batteries.Mapper.Interfaces;

namespace Batteries.Mapper.AppMapper;

public interface IAppMapper
{
    IConfigurationProvider ConfigurationProvider { get; }
    TDest AutoMap<TDest, TSource>(TSource source) where TDest : class, IAutoMapFrom<TSource, TDest> where TSource : class;
    TDest AutoMap<TSource, TDest>(TSource source, TDest dest) where TDest : class, IAutoMapFrom<TSource, TDest> where TSource : class;
    List<TDest> AutoMap<TDest, TSource>(List<TSource> source) where TDest : class, IAutoMapFrom<TSource, TDest> where TSource : class;
    Task<List<TDest>> Map<TDest, TSource>(List<TSource> source, CancellationToken ct) where TDest : class, IMapFrom<TSource, TDest> where TSource : class;
    Task<TDest> Map<TDest, TSource>(TSource source, CancellationToken ct) where TDest : class, IMapFrom<TSource, TDest> where TSource : class;
}