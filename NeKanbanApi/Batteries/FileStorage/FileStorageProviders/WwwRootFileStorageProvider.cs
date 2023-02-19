using Flurl;
using Microsoft.AspNetCore.Http;

namespace Batteries.FileStorage.FileStorageProviders;

public class WwwRootFileStorageProvider : IFileStorageProvider
{
    private readonly WebRootStorageConfig _storageConfig;
    public WwwRootFileStorageProvider(WebRootStorageConfig storageConfig)
    {
        _storageConfig = storageConfig;
    }

    public Task<string> GetAbsoluteUrl(string name, CancellationToken ct)
    {
        return Task.FromResult(Url.Combine(_storageConfig.StorageUrl, name));
    }

    public async Task Store(Stream stream, string name, CancellationToken ct)
    {
        await using var copyTo = new FileStream(Path.Combine(_storageConfig.StorageUrl, name), FileMode.Create);
        await stream.CopyToAsync(copyTo, ct);
    }

    public async Task<string> Store(IFormFile file, CancellationToken ct)
    {
        var name = $"{Guid.NewGuid()}.{Path.GetExtension(file.FileName)}";
        await Store(file.OpenReadStream(), name, ct);
        return name;
    }

    public Task Delete(string name, CancellationToken ct)
    {
        File.Delete(Path.Combine(_storageConfig.StorageUrl, name));
        return Task.CompletedTask;
    }
}