using AutoMapper;
using NeKanban.Common.Constants;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class ToDoUserVm : IMapSrcDest<ToDoUser,ToDoUserVm>
{
    public int Id { get; set; }
    public DeskUserLiteVm? DeskUser { get; set; }
    public ToDoUserType ToDoUserType { get; set; }
    public string ToDoUserTypeName => ToDoUserType.ToString();
    public static IMappingExpression<ToDoUser, ToDoUserVm> ConfigureMap(IMappingExpression<ToDoUser, ToDoUserVm> cfg)
    {
        return cfg;
    }
}