using AutoMapper;

namespace Batteries.Mapper.AppMapper.Extensions;

public static class MapperExtensions
{
    public static IMappingExpression<TSource, TDestination> IgnoreAllMembers<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expr)
    {
        var destinationType = typeof(TDestination);
        foreach (var property in destinationType.GetProperties())
        { 
            expr.ForMember(property.Name, opt => opt.Ignore());
        }

        return expr;
    }
}