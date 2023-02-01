using System.Collections;
using System.Reflection;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace NeKanban.Logic.Configuration;

public static class AutomapperConfiguration
{
    public static void AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
    }
    
    public static void ValidateAutomapperConfiguration(this IServiceProvider provider)
    {
        var mapper = provider.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    public static void ValidateOnlyNestedTypes(this IServiceProvider provider)
    {
        var mapper = provider.GetRequiredService<IMapper>();
        var allMaps = mapper.ConfigurationProvider.Internal().GetAllTypeMaps();
        var exceptions = new List<string>();
        foreach (var map in allMaps)
        {
            var src = map.SourceType;
            var dst = map.DestinationType;
            foreach (var propMap in map.PropertyMaps)
            {
                var sourceType = propMap.SourceType;
                var destinationType = propMap.DestinationType;
                
                if (sourceType == null || destinationType == null || sourceType == destinationType)
                {
                    continue;
                }
                
                if (!IsValidMap(allMaps, sourceType, destinationType))
                {
                    exceptions.Add($"No inner map {sourceType} -> {destinationType} configured for {src} -> {dst}");
                }
            }
        }

        if (exceptions.Any())
        {
            throw new Exception(string.Join('\n', exceptions));
        }
    }
    
    private static bool IsValidMap(IReadOnlyCollection<TypeMap> allMaps, Type src, Type dest)
    {
        var isDestCollection = typeof(IEnumerable).IsAssignableFrom(dest) && dest != typeof(string);
        var isSrcCollection = typeof(IEnumerable).IsAssignableFrom(src) && dest != typeof(string);
        if (isDestCollection != isSrcCollection)
        {
            return false;
        }

        if (isDestCollection)
        {
            src = src.GetTypeInfo().GenericTypeArguments[0];
            dest = dest.GetTypeInfo().GenericTypeArguments[0];
        }
        
        return allMaps.Any(x =>
            x.SourceType == src && x.DestinationType == dest);
    }
}