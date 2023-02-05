using AutoMapper;
using NeKanban.Common.DTOs.Roles;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class RoleVm : BaseEntityModel<int>, IMapFrom<Role, RoleVm>, IMapFrom<RoleDto, RoleVm>
{
    public required string Name { get; set; }
    public required bool IsDefault { get; set; }
    public static void ConfigureMap(IMappingExpression<Role, RoleVm> cfg)
    {
    }

    public static void ConfigureMap(IMappingExpression<RoleDto, RoleVm> cfg)
    {
    }
}