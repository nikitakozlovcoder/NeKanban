using AutoMapper;

namespace NeKanban.Common.Interfaces;

public interface IMapFrom<TSource, TDest> where TSource : new() where TDest : new()
{
    static abstract void ConfigureMap(IMappingExpression<TSource, TDest> cfg);
}