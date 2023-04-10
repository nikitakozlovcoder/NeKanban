using Microsoft.AspNetCore.Http;

namespace Batteries.FileStorage.DirectFileStorageAdapters;

public interface IDirectFileStorageAdapter
{
    Task<Guid> Store(IFormFile file, CancellationToken ct);
    Task<Guid> Store(Stream stream, string name, CancellationToken ct);
    
    Task Delete(Guid fileId, CancellationToken ct);
    Task Delete(IEnumerable<Guid> fileIds, CancellationToken ct);
    
    Task<string> GetUrl(Guid entityId, CancellationToken ct);
}