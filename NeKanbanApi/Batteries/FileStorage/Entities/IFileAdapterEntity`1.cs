using Batteries.Repository;

namespace Batteries.FileStorage.Entities;

public interface IFileAdapterEntity<TParent> : IFileAdapterEntity<TParent, FileStorageEntity>
    where TParent : IHasPk<int>
{
  
}