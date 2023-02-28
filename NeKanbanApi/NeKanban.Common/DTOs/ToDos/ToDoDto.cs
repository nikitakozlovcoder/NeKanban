using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.DTOs.ToDoUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.ViewModels.Columns;

namespace NeKanban.Common.DTOs.ToDos;

public class ToDoDto : BaseEntityModel<int>, IAutoMapFrom<ToDo, ToDoDto>
{
    public required string? Name { get; set; }
    public required int Order { get; set; }
    public required ColumnDto Column { get; set; }
    public required List<ToDoUserDto> ToDoUsers { get; set; }

    public static void ConfigureMap(IMappingExpression<ToDo, ToDoDto> cfg)
    {
    }
}