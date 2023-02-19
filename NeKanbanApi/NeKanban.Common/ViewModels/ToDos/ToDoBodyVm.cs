using Batteries.Mapper.Interfaces;
using NeKanban.Common.DTOs.ToDos;

namespace NeKanban.Common.ViewModels.ToDos;

public class ToDoBodyVm : IMapFrom<ToDoLiteDto, ToDoBodyVm>
{
    public required string Body { get; set; }
    //TODO images
}