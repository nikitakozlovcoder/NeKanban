using AutoMapper;

namespace NeKanban.Logic.Configuration;

public static class MappingExtensions
{
    public static IMappingExpression<TSource, TDest> ForMemberLight<TSource, TDest, TMember, TSourceMember>(this IMappingExpression<TSource, TDest> cfg, Func<TDest, TMember> member, Func<TSource, TSourceMember> sourceMember)
    {
        return cfg.ForMember(x => member(x), _ => _.MapFrom(x => sourceMember(x)));
    }
}