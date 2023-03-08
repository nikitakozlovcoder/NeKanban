using Batteries.FileStorage.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Batteries.FileStorage.DirectFileStorageAdapters;

public static class DirectFileStorageAdaptersExtensions
{
    public static void AddDirectFileStorageAdapter(this IServiceCollection collection)
    {
        AddDirectFileStorageAdapter<FileStorageEntity>(collection);
    }
    
    public static void AddDirectFileStorageAdapter<TFileEntity>(this IServiceCollection collection)
        where TFileEntity : class, IFileEntity, new()
    {
        collection.AddScoped<IDirectFileStorageAdapter, DirectFileStorageAdapter<TFileEntity>>();
        collection.AddScoped(typeof(IDirectFileStorageAdapter<>), typeof(DirectFileStorageAdapter<>));
    }
}