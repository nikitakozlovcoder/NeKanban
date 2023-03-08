namespace Batteries.FileStorage.Models;

public class FileStoreDto
{
    public required int Id { get; set; }
    public required Guid FileId { get; set; }
    public required string FileName { get; set; }
}