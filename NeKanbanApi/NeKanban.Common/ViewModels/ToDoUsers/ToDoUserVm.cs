using Batteries.Mapper.Interfaces;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.ToDoUsers;
using NeKanban.Common.ViewModels.DesksUsers;

namespace NeKanban.Common.ViewModels.ToDoUsers;

public class ToDoUserVm : IMapFrom<ToDoUserDto, ToDoUserVm>
{
    public required int Id { get; set; }
    public required DeskUserLiteVm? DeskUser { get; set; }
    public required ToDoUserType ToDoUserType { get; set; }
    public string ToDoUserTypeName => ToDoUserType.ToString();
}