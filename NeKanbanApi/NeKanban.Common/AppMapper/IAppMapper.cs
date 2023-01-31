using AutoMapper;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.AppMapper;

public interface IAppMapper
{
    IConfigurationProvider ConfigurationProvider { get; }
    TDest Map<TDest, TSource>(TSource source) where TDest : IMapFrom<TSource, TDest>, new() where TSource : new();
    TDest Map<TSource, TDest>(TSource source, TDest dest) where TDest : IMapFrom<TSource, TDest>, new() where TSource : new();
    List<TDest> Map<TDest, TSource>(List<TSource> source) where TDest : IMapFrom<TSource, TDest>, new() where TSource : new();
}