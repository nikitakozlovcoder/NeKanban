using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.DTOs.Roles;

public class RoleDto : BaseEntityModel<int>, IMapFrom<Role, RoleDto>
{
    public required string Name { get; set; }
    public required bool IsDefault { get; set; }
    public static void ConfigureMap(IMappingExpression<Role, RoleDto> cfg)
    {
    }
}