using System.Linq.Expressions;
using Batteries.FileStorage.Entities;
using Batteries.FileStorage.Models;
using Batteries.Repository;
using Microsoft.AspNetCore.Http;

namespace Batteries.FileStorage.FileStorageAdapters;

public interface IFileStorageAdapter<TFileEntity, TParent, in TFileStoreOptions>
    where TFileEntity :  class, IFileEntity<TParent>, new()
    where TParent : IHasPk<int>
{
    Task<TFileEntity> Store(int parentId, IFormFile file, CancellationToken ct);
    Task<TFileEntity> Store(int parentId, Stream stream, string name, CancellationToken ct);
    Task<TFileEntity> Store(int parentId, TFileStoreOptions options, CancellationToken ct);
    Task Delete(int fileId, CancellationToken ct);
    Task Delete(IEnumerable<int> fileIds, CancellationToken ct);
    Task<List<FileStoreUrlDto>> GetAllUrls(int parentId, CancellationToken ct);
    Task<List<FileStoreUrlDto>> GetAllUrls(Expression<Func<TFileEntity, bool>> predicate, CancellationToken ct);
}