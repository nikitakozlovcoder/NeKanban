using Batteries.FileStorage.Entities;

namespace Batteries.FileStorage.DirectFileStorageAdapters;

public interface IDirectFileStorageAdapter<TFileEntity> : IDirectFileStorageAdapter where TFileEntity : class, IFileEntity, new()
{
}