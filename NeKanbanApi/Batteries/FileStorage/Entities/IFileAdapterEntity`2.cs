using Batteries.Repository;

namespace Batteries.FileStorage.Entities;

public interface IFileAdapterEntity<TParent, TFileEntity> : IHasPk<int> 
    where TParent : IHasPk<int>
    where TFileEntity : IFileEntity
{
    int FileId { get; set; }
    TFileEntity File { get; set; }
    int? ParentId { get; set; }
    TParent? Parent { get; set; }
}