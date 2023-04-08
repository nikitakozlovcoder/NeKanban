using Microsoft.Extensions.DependencyInjection;

namespace Batteries.Repository.Extensions;

public static class GenericRepositoryConfigurationExtension
{
    public static void AddGenericRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IGuidRepository<>), typeof(GuidRepository<>));
    }
}