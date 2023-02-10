using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.DTOs.Roles;

public class RoleDto : BaseEntityModel<int>, IAutoMapFrom<Role, RoleDto>
{
    public required string Name { get; set; }
    public required bool IsDefault { get; set; }
    public required int DeskId { get; set; }
    public static void ConfigureMap(IMappingExpression<Role, RoleDto> cfg)
    {
    }
}