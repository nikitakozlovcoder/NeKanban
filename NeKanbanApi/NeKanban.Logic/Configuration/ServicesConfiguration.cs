using Batteries.Injection;
using Batteries.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace NeKanban.Logic.Configuration;

public static class ServicesConfiguration
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddMapper();
        services.AddAutoServices();
    }
}