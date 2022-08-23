using NeKanban.Data.Constants;

namespace NeKanban.Logic.Services.ViewModels;

public class ToDoUserVm
{
    public int Id { get; set; }
    public DeskUserLightVm? DeskUser { get; set; }
    public ToDoUserType ToDoUserType { get; set; }
    public string ToDoUserTypeName => ToDoUserType.ToString();
}