namespace NeKanban.Logic.Services.ViewModels;

public class ToDoVm : BaseIdVm
{
    public required string? Name { get; set; }
    public required int Order { get; set; }
    public required string? Body { get; set; }
    public required ColumnVm? Column { get; set; }
    public required List<ToDoUserVm> ToDoUsers { get; set; } = new();
}