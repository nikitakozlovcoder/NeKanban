using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.ViewModels;

namespace NeKanban.Common.DTOs.ToDoUsers;

public class ToDoUserDto : IAutoMapFrom<ToDoUser, ToDoUserDto>
{
    public required int Id { get; set; }
    public required DeskUserLiteDto? DeskUser { get; set; }
    public required ToDoUserType ToDoUserType { get; set; }
    public string ToDoUserTypeName => ToDoUserType.ToString();
    public static void ConfigureMap(IMappingExpression<ToDoUser, ToDoUserDto> cfg)
    {
    }
}