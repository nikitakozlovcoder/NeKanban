using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.Entities;

namespace NeKanban.Common.DTOs.ToDos;

public class ToDoFullDto : ToDoDto, IAutoMapFrom<ToDo, ToDoFullDto>
{
    public required string? Body { get; set; }
    public static void ConfigureMap(IMappingExpression<ToDo, ToDoFullDto> cfg)
    {
    }
}