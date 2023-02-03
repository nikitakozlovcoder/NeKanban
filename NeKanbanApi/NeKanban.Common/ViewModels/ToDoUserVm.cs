using AutoMapper;
using NeKanban.Common.Constants;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class ToDoUserVm : IMapFrom<ToDoUser,ToDoUserVm>
{
    public required int Id { get; set; }
    public required DeskUserLiteVm? DeskUser { get; set; }
    public required ToDoUserType ToDoUserType { get; set; }
    public string ToDoUserTypeName => ToDoUserType.ToString();
    public static void ConfigureMap(IMappingExpression<ToDoUser, ToDoUserVm> cfg)
    {
    }
}