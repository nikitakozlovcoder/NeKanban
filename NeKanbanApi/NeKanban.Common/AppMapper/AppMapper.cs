using AutoMapper;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using NeKanban.Common.Attributes;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.AppMapper;

[UsedImplicitly]
[Injectable<IAppMapper>]
public class AppMapper : IAppMapper
{
    public IConfigurationProvider ConfigurationProvider => _mapper.ConfigurationProvider;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _provider;
    public AppMapper(IMapper mapper, IServiceProvider provider)
    {
        _mapper = mapper;
        _provider = provider;
    }

    public TDest AutoMap<TDest, TSource>(TSource source) where TDest : class, IAutoMapFrom<TSource, TDest> where TSource : class
    {
        return _mapper.Map<TDest>(source);
    }

    public TDest AutoMap<TSource, TDest>(TSource source, TDest dest) where TDest : class, IAutoMapFrom<TSource, TDest> where TSource : class
    {
        return _mapper.Map(source, dest);
    }

    public List<TDest> AutoMap<TDest, TSource>(List<TSource> source) where TDest : class, IAutoMapFrom<TSource, TDest> where TSource : class
    {
        return _mapper.Map<List<TDest>>(source);
    }

    public async Task<List<TDest>> Map<TDest, TSource>(List<TSource> source, CancellationToken ct) where TDest : class, IMapFrom<TSource, TDest> where TSource : class
    {
        var profileType = typeof(IMappingProfile<,>).MakeGenericType(typeof(TSource), typeof(TDest));
        var profile = _provider.GetRequiredService(profileType);
        var createMap = profileType.GetMethods().Single(x => x is {Name: "Map"} && x.GetParameters().Length == 2
            && x.GetParameters().First().ParameterType == typeof(List<>).MakeGenericType(typeof(TSource))
            && x.ReturnType == typeof(Task<>).MakeGenericType(typeof(List<>).MakeGenericType(typeof(TDest))));
        var task = (Task)createMap.Invoke(profile, new object[]{source, ct})!;
        await task;
        var dest = task.GetType().GetProperty("Result")!.GetValue(task)!;
        return (List<TDest>) dest;
    }

    public async Task<TDest> Map<TDest, TSource>(TSource source, CancellationToken ct) where TDest : class, IMapFrom<TSource, TDest> where TSource : class
    {
        var profileType = typeof(IMappingProfile<,>).MakeGenericType(typeof(TSource), typeof(TDest));
        var profile = _provider.GetRequiredService(profileType);
        var createMap = profileType.GetMethods().Single(x => x is {Name: "Map"} && x.GetParameters().Length == 2
            && x.GetParameters().First().ParameterType == typeof(TSource) && x.ReturnType == typeof(Task<>).MakeGenericType(typeof(TDest)));
        var task = (Task)createMap.Invoke(profile, new object[]{source, ct})!;
        await task;
        var dest = task.GetType().GetProperty("Result")!.GetValue(task)!;
        return (TDest)dest;
    }
}