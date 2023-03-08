using Batteries.FileStorage.Entities;
using Batteries.FileStorage.FileStorageAdapters;
using Batteries.FileStorage.FileStorageProviders;
using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;

namespace NeKanban.Logic.Services.FileStorageAdapters;

[UsedImplicitly]
[Injectable<IFileStorageAdapter<ToDoFileAdapter, ToDo>>]
public class ToDoFileStorageAdapterService : BaseFileStorageAdapter<ToDoFileAdapter, ToDo>
{
    public ToDoFileStorageAdapterService(IFileStorageProvider provider,
        IRepository<ToDoFileAdapter> storeAdapterRepository,
        IGuidRepository<FileStorageEntity> fileRepository) : base(provider, storeAdapterRepository, fileRepository)
    {
    }
}