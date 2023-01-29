using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class ToDoVm : BaseIdVm, IMapSrcDest<ToDo, ToDoVm>
{
    public string? Name { get; set; }
    public int Order { get; set; }
    public string? Body { get; set; }
    public ColumnVm? Column { get; set; }
    public List<ToDoUserVm> ToDoUsers { get; set; } = new();
    public static IMappingExpression<ToDo, ToDoVm> ConfigureMap(IMappingExpression<ToDo, ToDoVm> cfg)
    {
        return cfg;
    }
}