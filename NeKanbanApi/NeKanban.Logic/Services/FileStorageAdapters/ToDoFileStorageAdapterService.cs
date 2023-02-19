using Batteries.FileStorage.FileStorageAdapters;
using Batteries.FileStorage.FileStorageProviders;
using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;

namespace NeKanban.Logic.Services.FileStorageAdapters;

[UsedImplicitly]
[Injectable<IFileStorageAdapter<ToDoFileAdapter, ToDo, FileEntity>>]
public class ToDoFileStorageAdapterService : BaseFileStorageAdapter<ToDoFileAdapter, ToDo, FileEntity>
{
    public ToDoFileStorageAdapterService(IFileStorageProvider provider, IRepository<ToDoFileAdapter> storeAdapterRepository, IRepository<FileEntity> fileRepository) : base(provider, storeAdapterRepository, fileRepository)
    {
    }
}