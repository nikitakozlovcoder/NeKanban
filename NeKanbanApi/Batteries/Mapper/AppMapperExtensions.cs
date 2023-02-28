using AutoMapper;
using Batteries.Mapper.AppMapper.Extensions;
using Batteries.Mapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Batteries.Mapper;

public static class AppMapperExtensions
{
    public static void AddMapper(this IServiceCollection services, params Type[] profiles)
    {
        var profilesToUse = new List<Type>{typeof(MappingProfile)};
        profilesToUse.AddRange(profiles);
        services.AddAutoMapper(profilesToUse.ToArray());
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
            var baseTypes = type.GetParents();
            foreach (var typeInterface in interfaces.Where(x => !baseTypes.Any(x.IsAssignableFrom)))
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
}