using Batteries.FileStorage.Entities;

namespace NeKanban.Common.Entities;

public class CommentFileAdapter : IFileAdapterEntity<Comment>
{
    public int Id { get; set; }
    public Guid FileId { get; set; }
    public int? ParentId { get; set; }
    public virtual Comment? Parent { get; set; }
    public virtual FileStorageEntity File { get; set; } = null!;
}