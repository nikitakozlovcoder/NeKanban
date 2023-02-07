using AutoMapper;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.AppMapper;

public interface IAppMapper
{
    IConfigurationProvider ConfigurationProvider { get; }
    TDest Map<TDest, TSource>(TSource source) where TDest : class, IAutoMapFrom<TSource, TDest> where TSource : class;
    TDest Map<TSource, TDest>(TSource source, TDest dest) where TDest : class, IAutoMapFrom<TSource, TDest> where TSource : class;
    List<TDest> Map<TDest, TSource>(List<TSource> source) where TDest : class, IAutoMapFrom<TSource, TDest> where TSource : class;
}