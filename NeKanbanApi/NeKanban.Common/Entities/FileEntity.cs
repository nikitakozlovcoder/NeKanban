using Batteries.FileStorage.Entities;

namespace NeKanban.Common.Entities;

public class FileEntity : IFileEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}