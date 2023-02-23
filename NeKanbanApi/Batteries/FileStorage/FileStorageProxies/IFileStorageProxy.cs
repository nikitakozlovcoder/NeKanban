﻿namespace Batteries.FileStorage.FileStorageProxies;

public interface IFileStorageProxy
{
    Task<string> GetAbsoluteUrl(string name, CancellationToken ct);
    Task<string> GetAbsoluteUrl(int id, CancellationToken ct);
    Task<string> GetProxyUrl(int id, CancellationToken ct);
}