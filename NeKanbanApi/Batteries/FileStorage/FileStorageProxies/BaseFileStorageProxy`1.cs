using Batteries.FileStorage.Entities;
using Batteries.FileStorage.FileStorageProviders;
using Batteries.Repository;
using Flurl;

namespace Batteries.FileStorage.FileStorageProxies;

public class BaseFileStorageProxy<TFileEntity> : IFileStorageProxy<TFileEntity>
    where TFileEntity : class, IFileEntity
{
    private readonly IGuidRepository<TFileEntity> _fileRepository;
    private readonly IFileStorageProvider _provider;
    private readonly FileStorageProxyConfig _config;
    public BaseFileStorageProxy(IGuidRepository<TFileEntity> fileRepository,
        IFileStorageProvider provider,
        FileStorageProxyConfig config)
    {
        _fileRepository = fileRepository;
        _provider = provider;
        _config = config;
    }

    public virtual Task<string> GetAbsoluteUrl(string name, CancellationToken ct)
    {
        return _provider.GetAbsoluteUrl(name, ct);
    }
    
    public virtual async Task<string> GetAbsoluteUrl(Guid id, CancellationToken ct)
    {
        var name = await _fileRepository.Single(x => x.Id == id, x => x.Name, ct);
        return await GetAbsoluteUrl(name, ct);
    }

    public virtual string GetProxyUrl(string name)
    {
        return Url.Combine(_config.ProxyEndpoint, name);
    }
}