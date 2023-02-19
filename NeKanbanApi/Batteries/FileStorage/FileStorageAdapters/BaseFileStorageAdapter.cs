using System.Linq.Expressions;
using Batteries.FileStorage.Entities;
using Batteries.FileStorage.FileStorageProviders;
using Batteries.FileStorage.Models;
using Batteries.Repository;
using Microsoft.AspNetCore.Http;

namespace Batteries.FileStorage.FileStorageAdapters;

public abstract class BaseFileStorageAdapter<TFileEntity, TParent, TFileStoreOptions> 
    : IFileStorageAdapter<TFileEntity, TParent, TFileStoreOptions>
    where TFileEntity : class, IFileEntity<TParent>, new()
    where TParent : IHasPk<int>
{
    private readonly IFileStorageProvider _provider;
    private readonly IRepository<TFileEntity> _storeRepository;

    public BaseFileStorageAdapter(IFileStorageProvider provider, IRepository<TFileEntity> storeRepository)
    {
        _provider = provider;
        _storeRepository = storeRepository;
    }

    public virtual async Task<TFileEntity> Store(int parentId, IFormFile file, CancellationToken ct)
    {
        var name = await _provider.Store(file, ct);
        var storeEntity = new TFileEntity
        {       
            Name = name,
            ParentId = parentId
        };

        await _storeRepository.Create(storeEntity, ct);
        return storeEntity;
    }

    public virtual async Task<TFileEntity> Store(int parentId, Stream stream, string name, CancellationToken ct)
    {
        await _provider.Store(stream, name, ct);
        var storeEntity = new TFileEntity
        {       
            Name = name,
            ParentId = parentId
        };

        await _storeRepository.Create(storeEntity, ct);
        return storeEntity;
    }

    public abstract Task<TFileEntity> Store(int parentId, TFileStoreOptions options, CancellationToken ct);
    
    public async Task Delete(int fileId, CancellationToken ct)
    {
        await Delete(new[] {fileId}, ct);
    }

    public virtual async Task Delete(IEnumerable<int> fileIds, CancellationToken ct)
    {
        var files = await _storeRepository.ToList(x => fileIds.Contains(x.Id), ct);
        foreach (var file in files)
        {
            await _provider.Delete(file.Name, ct);
        }
      
        await _storeRepository.Remove(files, ct);
    }

    public Task<List<FileStoreUrlDto>> GetAllUrls(int parentId, CancellationToken ct)
    {
        return GetAllUrls(x => x.ParentId == parentId, ct);
    }

    public virtual async Task<List<FileStoreUrlDto>> GetAllUrls(Expression<Func<TFileEntity, bool>> predicate, CancellationToken ct)
    {
        var files = await _storeRepository.ToList(predicate, ent => new
            {
               ent.Id,
               ent.Name
            }, ct: ct);

        var result = new List<FileStoreUrlDto>();
        foreach (var file in files)
        {
            var absoluteUrl = await _provider.GetAbsoluteUrl(file.Name, ct);
            result.Add(new FileStoreUrlDto
            {
                Id = file.Id,
                Url = absoluteUrl
            });
        }

        return result;
    }
}
    