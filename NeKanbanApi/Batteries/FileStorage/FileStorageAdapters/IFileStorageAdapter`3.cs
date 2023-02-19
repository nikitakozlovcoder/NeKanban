using System.Linq.Expressions;
using Batteries.FileStorage.Entities;
using Batteries.FileStorage.Models;
using Batteries.Repository;
using Microsoft.AspNetCore.Http;

namespace Batteries.FileStorage.FileStorageAdapters;

public interface IFileStorageAdapter<TFileAdapterEntity, TParent, TFileEntity>
    where TFileAdapterEntity :  class, IFileAdapterEntity<TParent, TFileEntity>, new()
    where TParent : IHasPk<int>
    where TFileEntity : IFileEntity
{
    Task<TFileAdapterEntity> Store(int parentId, IFormFile file, CancellationToken ct);
    Task<TFileAdapterEntity> Store(int parentId, Stream stream, string name, CancellationToken ct);
    Task Delete(int fileId, CancellationToken ct);
    Task Delete(IEnumerable<int> fileIds, CancellationToken ct);
    Task<List<FileStoreUrlDto>> GetAllUrls(int parentId, CancellationToken ct);
    Task<List<FileStoreUrlDto>> GetAllUrls(Expression<Func<TFileAdapterEntity, bool>> predicate, CancellationToken ct);
    Task<FileStoreUrlDto> GetUrl(int entityId, CancellationToken ct);
}