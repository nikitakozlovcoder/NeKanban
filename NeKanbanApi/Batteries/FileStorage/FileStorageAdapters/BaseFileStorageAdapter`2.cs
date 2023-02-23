using Batteries.FileStorage.Entities;
using Batteries.FileStorage.FileStorageProviders;
using Batteries.Repository;

namespace Batteries.FileStorage.FileStorageAdapters;

public abstract class BaseFileStorageAdapter<TFileAdapterEntity, TParent> : BaseFileStorageAdapter<TFileAdapterEntity, TParent, FileStorageEntity>,
    IFileStorageAdapter<TFileAdapterEntity, TParent>
    where TFileAdapterEntity : class, IFileAdapterEntity<TParent>, new()
    where TParent : IHasPk<int>
{
    protected BaseFileStorageAdapter(IFileStorageProvider provider, IRepository<TFileAdapterEntity> storeAdapterRepository, IRepository<FileStorageEntity> fileRepository) : base(provider, storeAdapterRepository, fileRepository)
    {
    }
}