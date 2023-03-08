using Microsoft.AspNetCore.Http;

namespace Batteries.FileStorage.FileStorageProviders;

public interface IFileStorageProvider
{
    Task<string> GetAbsoluteUrl(string name, CancellationToken ct);
    Task Store(Stream stream, string name, CancellationToken ct);
    Task<string> Store(IFormFile file, CancellationToken ct);
    Task Delete(string name, CancellationToken ct);
}