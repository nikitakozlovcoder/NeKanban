namespace NeKanban.Logic.Services.ViewModels;

public class ToDoVm : BaseIdVm
{
    public string? Name { get; set; }
    public int Order { get; set; }
    public string? Body { get; set; }
    public ColumnVm? Column { get; set; }
    public List<ToDoUserVm> ToDoUsers { get; set; } = new();
}