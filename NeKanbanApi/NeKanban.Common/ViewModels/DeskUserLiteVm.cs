using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;
using NeKanban.Security.Constants;

namespace NeKanban.Common.ViewModels;

public class DeskUserLiteVm: BaseIdVm, IMapSrcDest<DeskUser, DeskUserLiteVm>
{
    public ApplicationUserVm? User { get; set; }
    public RoleType Role { get; set; }
    public string RoleName => Role.ToString();
    public static IMappingExpression<DeskUser, DeskUserLiteVm> ConfigureMap(IMappingExpression<DeskUser, DeskUserLiteVm> cfg)
    {
        return cfg;
    }
}