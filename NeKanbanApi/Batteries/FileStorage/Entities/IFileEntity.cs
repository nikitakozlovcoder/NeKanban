using Batteries.Repository;

namespace Batteries.FileStorage.Entities;

public interface IFileEntity : IHasPk<Guid>
{
    public string Name { get; set; }
}