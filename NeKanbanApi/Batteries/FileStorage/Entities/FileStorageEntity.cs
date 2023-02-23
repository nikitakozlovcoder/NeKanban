namespace Batteries.FileStorage.Entities;

public class FileStorageEntity : IFileEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}