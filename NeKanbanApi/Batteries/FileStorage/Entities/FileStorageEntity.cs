namespace Batteries.FileStorage.Entities;

public class FileStorageEntity : IFileEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}