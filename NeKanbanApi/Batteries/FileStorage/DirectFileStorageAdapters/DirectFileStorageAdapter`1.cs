using Batteries.FileStorage.Entities;
using Batteries.FileStorage.FileStorageProviders;
using Batteries.Repository;
using Microsoft.AspNetCore.Http;

namespace Batteries.FileStorage.DirectFileStorageAdapters;

public class DirectFileStorageAdapter<TFileEntity> : IDirectFileStorageAdapter<TFileEntity> where TFileEntity : class, IFileEntity, new()
{
    private readonly IFileStorageProvider _provider;
    private readonly IGuidRepository<TFileEntity> _fileRepository;

    public DirectFileStorageAdapter(IFileStorageProvider provider, IGuidRepository<TFileEntity> fileRepository)
    {
        _provider = provider;
        _fileRepository = fileRepository;
    }

    public virtual async Task<Guid> Store(IFormFile file, CancellationToken ct)
    {
        var name = await _provider.Store(file, ct);
        return await CreateEntity(name, ct);
    }

    public virtual async Task<Guid> Store(Stream stream, string name, CancellationToken ct)
    {
        await _provider.Store(stream, name, ct);
        return await CreateEntity(name, ct);
    }

    public virtual Task Delete(Guid fileId, CancellationToken ct)
    {
        return Delete(new[] {fileId}, ct);
    }

    public virtual async Task Delete(IEnumerable<Guid> fileIds, CancellationToken ct)
    {
        var names = await _fileRepository.ToList(x => fileIds.Contains(x.Id), x => x.Name, ct: ct);
        foreach (var name in names)
        {
            await _provider.Delete(name, ct);
        }
    }

    public virtual async Task<string> GetUrl(Guid entityId, CancellationToken ct)
    {
        var name = await _fileRepository.Single(x => x.Id == entityId, x => x.Name, ct);
        return await _provider.GetAbsoluteUrl(name, ct);
    }
    
    private async Task<Guid> CreateEntity(string name, CancellationToken ct)
    {
        var entity = new TFileEntity
        {
            Name = name
        };

        await _fileRepository.Create(entity, ct);
        return entity.Id;
    }
}