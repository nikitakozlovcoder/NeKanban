using System.Collections;
using System.Reflection;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.Extensions.DependencyInjection;
using NeKanban.Common.Interfaces;

namespace NeKanban.Logic.Configuration;

public static class MappingConfiguration
{
    public static void AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
    }
    
    public static void ValidateMapperConfiguration(this IServiceProvider provider)
    {
        var mapper = provider.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
        ValidateServiceMappings(provider);
    }

    private static void ValidateServiceMappings(IServiceProvider provider)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
        using var scope = provider.CreateScope();
        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            foreach (var typeInterface in interfaces)
            {
                if (!(typeInterface.IsGenericType &&
                      typeInterface.GetGenericTypeDefinition() == typeof(IMapFrom<,>)))
                {
                    continue;
                }

                var src = typeInterface.GetGenericArguments()[0];
                var dst = typeInterface.GetGenericArguments()[1];
                if (dst != type)
                {
                    throw new Exception($"{type} can`t define map configuration for {src}");
                }
                
                var mappingProfileType = typeof(IMappingProfile<,>).MakeGenericType(src, dst);
                var profile = scope.ServiceProvider.GetService(mappingProfileType);
                if (profile == null)
                {
                    throw new Exception($"No mapping profile defined for {src} -> {dst} service map");
                }
            }
        }
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