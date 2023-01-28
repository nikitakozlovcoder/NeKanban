using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NeKanban.Common.Attributes;

namespace NeKanban.Logic.Configuration;

public static class ServicesConfiguration
{
    public static void AddServices(this IServiceCollection services)
    {
        AddAutoServices(services);
    }

    private static void AddAutoServices(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<InjectableAttribute>();
                if (attr == null)
                {
                    continue;
                }

                var abstractions = attr.Abstractions;
                if (abstractions.Any())
                {
                    foreach (var abstraction in abstractions)
                    {
                        services.AddScoped(abstraction, type);
                    }
                }
                else
                {
                    services.AddScoped(type);
                }
            }
        }
    }
}