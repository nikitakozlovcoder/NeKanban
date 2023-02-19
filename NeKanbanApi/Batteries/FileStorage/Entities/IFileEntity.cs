using Batteries.Repository;

namespace Batteries.FileStorage.Entities;

public interface IFileEntity : IHasPk<int>
{
    public string Name { get; set; }
}