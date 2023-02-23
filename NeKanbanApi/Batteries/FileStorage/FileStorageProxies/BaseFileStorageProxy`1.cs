using Batteries.FileStorage.Entities;
using Batteries.FileStorage.FileStorageProviders;
using Batteries.Repository;
using Flurl;

namespace Batteries.FileStorage.FileStorageProxies;

public class BaseFileStorageProxy<TFileEntity> : IFileStorageProxy<TFileEntity>
    where TFileEntity : class, IFileEntity
{
    private readonly IRepository<TFileEntity> _fileRepository;
    private readonly IFileStorageProvider _provider;
    private readonly FileStorageProxyConfig _config;
    public BaseFileStorageProxy(IRepository<TFileEntity> fileRepository,
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
    
    public virtual async Task<string> GetAbsoluteUrl(int id, CancellationToken ct)
    {
        var name = await _fileRepository.Single(x => x.Id == id, x => x.Name, ct);
        return await GetAbsoluteUrl(name, ct);
    }

    public virtual Task<string> GetProxyUrl(int id, CancellationToken ct)
    {
        return Task.FromResult(Url.Combine(_config.ProxyEndpoint, id.ToString()));
    }
}