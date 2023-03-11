namespace NeKanban.Common.Models.ToDoModels;

public class ToDoUpdateModel
{
    public required string Name { get; set; }
    public string? Body { get; set; }
}