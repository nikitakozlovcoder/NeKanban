using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;
using NeKanban.Security.Constants;

namespace NeKanban.Common.DTOs.Permissions;

public class RolePermissionDto : BaseEntityModel<int>, IAutoMapFrom<RolePermission, RolePermissionDto>
{
    public required PermissionType Permission { get; set; }
    public string PermissionName => Permission.ToString();
    public static void ConfigureMap(IMappingExpression<RolePermission, RolePermissionDto> cfg)
    {
    }
}