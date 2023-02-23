using Batteries.FileStorage.Entities;

namespace NeKanban.Common.Entities;

public class ToDoFileAdapter : IFileAdapterEntity<ToDo>
{
    public int Id { get; set; }
    public int FileId { get; set; }
    public int? ParentId { get; set; }
    public virtual ToDo? Parent { get; set; }
    public virtual FileStorageEntity File { get; set; } = null!;
}