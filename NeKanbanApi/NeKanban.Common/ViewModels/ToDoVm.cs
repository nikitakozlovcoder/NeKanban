using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class ToDoVm : BaseIdVm, IMapFrom<ToDo, ToDoVm>
{
    public required string? Name { get; set; }
    public required int Order { get; set; }
    public required string? Body { get; set; }
    public required ColumnVm? Column { get; set; }
    public required List<ToDoUserVm> ToDoUsers { get; set; } = new();
    public static void ConfigureMap(IMappingExpression<ToDo, ToDoVm> cfg)
    {
    }
}