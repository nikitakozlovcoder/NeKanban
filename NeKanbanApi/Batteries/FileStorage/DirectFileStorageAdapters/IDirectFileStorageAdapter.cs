using Batteries.FileStorage.Entities;
using Microsoft.AspNetCore.Http;

namespace Batteries.FileStorage.DirectFileStorageAdapters;

public interface IDirectFileStorageAdapter : IDirectFileStorageAdapter<FileStorageEntity>
{
}