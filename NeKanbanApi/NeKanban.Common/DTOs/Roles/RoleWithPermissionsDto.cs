using AutoMapper;
using NeKanban.Common.DTOs.Permissions;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.DTOs.Roles;

public class RoleWithPermissionsDto : BaseEntityModel<int>, IAutoMapFrom<Role, RoleWithPermissionsDto>
{ 
    public required string Name { get; set; }
    public required bool IsDefault { get; set; }
    public required List<RolePermissionDto> Permissions { get; set; }
    public static void ConfigureMap(IMappingExpression<Role, RoleWithPermissionsDto> cfg)
    {
    }
}