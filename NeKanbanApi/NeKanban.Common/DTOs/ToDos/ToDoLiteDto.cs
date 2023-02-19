namespace NeKanban.Common.DTOs.ToDos;

public class ToDoLiteDto : BaseEntityModel<int>
{
    public required string? Body { get; set; }
}