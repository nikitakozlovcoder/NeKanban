using AutoMapper;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class DeskUserLiteVm: BaseIdVm, IMapFrom<DeskUser, DeskUserLiteVm>, IMapFrom<DeskUserLiteDto, DeskUserLiteVm>
{
    public required ApplicationUserVm? User { get; set; }
    public required RoleVm Role { get; set; }
    public required bool IsOwner { get; set; }
    public static void ConfigureMap(IMappingExpression<DeskUser, DeskUserLiteVm> cfg)
    {
    }

    public static void ConfigureMap(IMappingExpression<DeskUserLiteDto, DeskUserLiteVm> cfg)
    {
    }
}