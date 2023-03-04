using Batteries.FileStorage.Entities;
using Batteries.FileStorage.FileStorageProviders;
using Batteries.Repository;

namespace Batteries.FileStorage.FileStorageProxies;

public class BaseFileStorageProxy : BaseFileStorageProxy<FileStorageEntity>
{
    public BaseFileStorageProxy(IGuidRepository<FileStorageEntity> fileRepository,
        IFileStorageProvider provider,
        FileStorageProxyConfig config) : base(fileRepository, provider, config)
    {
    }
}