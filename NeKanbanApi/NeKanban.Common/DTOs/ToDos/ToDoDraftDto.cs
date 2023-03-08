using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.Entities;

namespace NeKanban.Common.DTOs.ToDos;

public class ToDoDraftDto : BaseEntityModel<int>, IAutoMapFrom<ToDo, ToDoDraftDto>
{
    public required string Name { get; set; }
    public required string Body { get; set; }
    public static void ConfigureMap(IMappingExpression<ToDo, ToDoDraftDto> cfg)
    {
    }
}