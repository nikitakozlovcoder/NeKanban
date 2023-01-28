using NeKanban.Data.Constants;

namespace NeKanban.Logic.Services.ViewModels;

public class ToDoUserVm
{
    public int Id { get; set; }
    public DeskUserLiteVm? DeskUser { get; set; }
    public ToDoUserType ToDoUserType { get; set; }
    public string ToDoUserTypeName => ToDoUserType.ToString();
}