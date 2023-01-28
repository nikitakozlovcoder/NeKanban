using NeKanban.Data.Constants;

namespace NeKanban.Logic.Services.ViewModels;

public class ToDoUserVm
{
    public required int Id { get; set; }
    public required DeskUserLiteVm? DeskUser { get; set; }
    public required ToDoUserType ToDoUserType { get; set; }
    public string ToDoUserTypeName => ToDoUserType.ToString();
}