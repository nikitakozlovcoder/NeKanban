
namespace Batteries.FileStorage.Models;

public class FileStoreUrlDto
{
    public required int Id { get; set; }
    public required string Url { get; set; }
    public required int FileId { get; set; }
}