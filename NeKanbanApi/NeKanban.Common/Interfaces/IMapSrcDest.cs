using AutoMapper;

namespace NeKanban.Common.Interfaces;

public interface IMapSrcDest<TSource, TDest> where TSource : new() where TDest : new()
{
    static abstract IMappingExpression<TSource, TDest> ConfigureMap(IMappingExpression<TSource, TDest> cfg);
}