using Batteries.FileStorage.Entities;

namespace Batteries.FileStorage.DirectFileStorageAdapters;

// ReSharper disable once UnusedTypeParameter
public interface IDirectFileStorageAdapter<TFileEntity> : IDirectFileStorageAdapter where TFileEntity : class, IFileEntity, new()
{
}