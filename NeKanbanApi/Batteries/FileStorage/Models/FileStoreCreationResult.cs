namespace Batteries.FileStorage.Models;

public class FileStoreCreationResult<TFileAdapterEntity>
{
    public required TFileAdapterEntity FileAdapter { get; set; }
    public required string FileName { get; set; }
}