using AutoMapper;

namespace NeKanban.Common.Interfaces;

public interface IMapFrom<TSource, TDest> where TSource : class where TDest : class
{
    static abstract void ConfigureMap(IMappingExpression<TSource, TDest> cfg);
}