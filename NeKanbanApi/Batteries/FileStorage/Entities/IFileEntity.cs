using Batteries.Repository;

namespace Batteries.FileStorage.Entities;

public interface IFileEntity<TParent> : IHasPk<int> where TParent : IHasPk<int>
{
    string Name { get; set; }
    int? ParentId { get; set; }
    TParent? Parent { get; set; }
}