using Microsoft.Extensions.DependencyInjection;

namespace Batteries.FileStorage.FileStorageProviders;

public static class WwwRootFileStorageExtensions
{
    public static void AddWwwRootStorage(this IServiceCollection collection, WebRootStorageConfig webRootPath)
    { 
        collection.AddSingleton<WebRootStorageConfig>(_ => webRootPath);
        collection.AddScoped<IFileStorageProvider, WwwRootFileStorageProvider>();
    }
}