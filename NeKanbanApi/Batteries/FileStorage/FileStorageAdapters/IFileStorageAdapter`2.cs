using Batteries.FileStorage.Entities;
using Batteries.Repository;

namespace Batteries.FileStorage.FileStorageAdapters;

public interface IFileStorageAdapter<TFileAdapterEntity, TParent> : IFileStorageAdapter<TFileAdapterEntity, TParent, FileStorageEntity>
    where TFileAdapterEntity : class, IFileAdapterEntity<TParent>, IFileAdapterEntity<TParent, FileStorageEntity>, new()
    where TParent : IHasPk<int>
{
   
}