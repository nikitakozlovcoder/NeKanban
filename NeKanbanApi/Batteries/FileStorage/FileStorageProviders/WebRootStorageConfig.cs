namespace Batteries.FileStorage.FileStorageProviders;

public class WebRootStorageConfig
{
    public required string Root { get; set; }
    public required string Folder { get; set; }
    public required string HostingUrl { get; set; }
    public string StorageUrl => Path.Combine(Root, Folder);
}