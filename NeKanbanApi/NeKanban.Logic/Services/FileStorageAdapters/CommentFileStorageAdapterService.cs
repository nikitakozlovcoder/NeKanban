using Batteries.FileStorage.Entities;
using Batteries.FileStorage.FileStorageAdapters;
using Batteries.FileStorage.FileStorageProviders;
using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;

namespace NeKanban.Logic.Services.FileStorageAdapters;

[UsedImplicitly]
[Injectable<IFileStorageAdapter<CommentFileAdapter, Comment>>]
public class CommentFileStorageAdapterService : BaseFileStorageAdapter<CommentFileAdapter, Comment>
{
    public CommentFileStorageAdapterService(IFileStorageProvider provider,
        IRepository<CommentFileAdapter> storeAdapterRepository,
        IGuidRepository<FileStorageEntity> fileRepository) : base(provider, storeAdapterRepository, fileRepository)
    {
    }
}