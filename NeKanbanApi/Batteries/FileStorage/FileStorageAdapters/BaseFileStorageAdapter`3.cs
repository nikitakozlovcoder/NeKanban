using System.Linq.Expressions;
using Batteries.FileStorage.Entities;
using Batteries.FileStorage.FileStorageProviders;
using Batteries.FileStorage.Models;
using Batteries.Repository;
using Microsoft.AspNetCore.Http;

namespace Batteries.FileStorage.FileStorageAdapters;

public abstract class BaseFileStorageAdapter<TFileAdapterEntity, TParent, TFileEntity> 
    : IFileStorageAdapter<TFileAdapterEntity, TParent, TFileEntity>
    where TFileAdapterEntity : class, IFileAdapterEntity<TParent, TFileEntity>, new()
    where TParent : IHasPk<int>
    where TFileEntity : class, IFileEntity, new()
{
    private readonly IFileStorageProvider _provider;
    private readonly IRepository<TFileAdapterEntity> _storeAdapterRepository;
    private readonly IGuidRepository<TFileEntity> _fileRepository;

    protected BaseFileStorageAdapter(IFileStorageProvider provider,
        IRepository<TFileAdapterEntity> storeAdapterRepository,
        IGuidRepository<TFileEntity> fileRepository)
    {
        _provider = provider;
        _storeAdapterRepository = storeAdapterRepository;
        _fileRepository = fileRepository;
    }

    public virtual async Task<TFileAdapterEntity> Store(int parentId, IFormFile file, CancellationToken ct)
    {
        var name = await _provider.Store(file, ct);
        return await CreateEntity(name, parentId, ct);
    }

    public virtual async Task<TFileAdapterEntity> Store(int parentId, Stream stream, string name, CancellationToken ct)
    {
        await _provider.Store(stream, name, ct);
        return await CreateEntity(name, parentId, ct);
    }

    public async Task Delete(int adapterId, CancellationToken ct)
    {
        await Delete(new[] {adapterId}, ct);
    }

    public virtual async Task Delete(IEnumerable<int> adapterIds, CancellationToken ct)
    {
        var files = await _storeAdapterRepository.ToList(x => adapterIds.Contains(x.Id), 
            x => new TFileEntity
            {
                Id = x.File.Id,
                Name = x.File.Name
            }, ct: ct);
        
        foreach (var file in files)
        {
            await _provider.Delete(file.Name, ct);
        }
      
        await _fileRepository.Remove(files, ct);
    }

    public async Task<FileStoreDto> Attach(int parentId, Guid fileId, CancellationToken ct)
    {

        return (await Attach(parentId, new[] {fileId}, ct)).Single();
       
    }

    public async Task<List<FileStoreDto>> Attach(int parentId, IEnumerable<Guid> fileIds, CancellationToken ct)
    {
        var adapters = fileIds.Select(fileId => new TFileAdapterEntity
        {
            FileId = fileId,
            ParentId = parentId
        }).ToList();

        await _storeAdapterRepository.Create(adapters, ct);
        return adapters.Select(x => new FileStoreDto
        {
            Id = x.Id,
            FileId = x.FileId
        }).ToList();
    }

    public Task<List<FileStoreUrlDto>> GetAllUrls(int parentId, CancellationToken ct)
    {
        return GetAllUrls(x => x.ParentId == parentId, ct);
    }

    public Task<List<FileStoreDto>> GetAll(int parentId, CancellationToken ct)
    {
        return GetAll(x => x.ParentId == parentId, ct);
    }

    public virtual async Task<List<FileStoreUrlDto>> GetAllUrls(Expression<Func<TFileAdapterEntity, bool>> predicate, CancellationToken ct)
    {
        var files = await _storeAdapterRepository.ToList(predicate, ent => new
            {
               ent.Id,
               ent.FileId,
               ent.File.Name
            }, ct: ct);

        var result = new List<FileStoreUrlDto>();
        foreach (var file in files)
        {
            var absoluteUrl = await _provider.GetAbsoluteUrl(file.Name, ct);
            result.Add(new FileStoreUrlDto
            {
                Id = file.Id,
                FileId = file.FileId,
                Url = absoluteUrl
            });
        }

        return result;
    }

    public async Task<List<FileStoreDto>> GetAll(Expression<Func<TFileAdapterEntity, bool>> predicate, CancellationToken ct)
    {
        var files = await _storeAdapterRepository.ToList(predicate, ent => new FileStoreDto()
        {
            Id = ent.Id,
            FileId = ent.FileId,
        }, ct: ct);

        return files;
    }

    public async Task<FileStoreUrlDto> GetUrl(int entityId, CancellationToken ct)
    {
        var dtos = await GetAllUrls(x => x.Id == entityId, ct);
        return dtos.Single();
    }
    
    private async Task<TFileAdapterEntity> CreateEntity(string name, int parentId, CancellationToken ct)
    {
        var storeAdapterEntity = new TFileAdapterEntity
        {
            File = new TFileEntity
            {
                Name = name
            },
            ParentId = parentId
        };

        await _storeAdapterRepository.CreateRecursive(storeAdapterEntity, ct);
        return storeAdapterEntity;
    }
}