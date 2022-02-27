using NeKanban.Constants;
using NeKanban.Data.Entities;

namespace NeKanban.Services.ViewModels;

public class ToDoUserVm
{
    public DeskUserLightVm? DeskUser { get; set; }
    public ToDoUserType ToDoUserType { get; set; }
    public string ToDoUserTypeName => ToDoUserType.ToString();
}