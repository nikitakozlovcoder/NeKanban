﻿using System.Linq.Expressions;
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
    private readonly IRepository<TFileEntity> _fileRepository;

    protected BaseFileStorageAdapter(IFileStorageProvider provider, IRepository<TFileAdapterEntity> storeAdapterRepository, IRepository<TFileEntity> fileRepository)
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

    public async Task Delete(int fileId, CancellationToken ct)
    {
        await Delete(new[] {fileId}, ct);
    }

    public virtual async Task Delete(IEnumerable<int> fileIds, CancellationToken ct)
    {
        var files = await _storeAdapterRepository.ToList(x => fileIds.Contains(x.Id), 
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

    public Task<List<FileStoreUrlDto>> GetAllUrls(int parentId, CancellationToken ct)
    {
        return GetAllUrls(x => x.ParentId == parentId, ct);
    }

    public virtual async Task<List<FileStoreUrlDto>> GetAllUrls(Expression<Func<TFileAdapterEntity, bool>> predicate, CancellationToken ct)
    {
        var files = await _storeAdapterRepository.ToList(predicate, ent => new
            {
               ent.Id,
               ent.File.Name
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