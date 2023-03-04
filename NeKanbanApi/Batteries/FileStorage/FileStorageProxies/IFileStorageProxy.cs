namespace Batteries.FileStorage.FileStorageProxies;

public interface IFileStorageProxy
{
    Task<string> GetAbsoluteUrl(string name, CancellationToken ct);
    Task<string> GetAbsoluteUrl(Guid id, CancellationToken ct);
    string GetProxyUrl(Guid id);
}