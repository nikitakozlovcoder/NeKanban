using Batteries.FileStorage.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Batteries.FileStorage.FileStorageProxies;

public static class FileStorageExtensions
{
    public static void AddFileStorageProxy(this IServiceCollection collection, FileStorageProxyConfig config)
    {
        collection.AddFileStorageProxy<FileStorageEntity>(config);
    }
    
    public static void AddFileStorageProxy<TFileEntity>(this IServiceCollection collection, FileStorageProxyConfig config)
        where TFileEntity : class, IFileEntity
    {
        collection.AddSingleton<FileStorageProxyConfig>(_ => config);
        collection.AddScoped<IFileStorageProxy, BaseFileStorageProxy<TFileEntity>>();
        collection.AddScoped(typeof(IFileStorageProxy<>), typeof(BaseFileStorageProxy<>));
    }
}