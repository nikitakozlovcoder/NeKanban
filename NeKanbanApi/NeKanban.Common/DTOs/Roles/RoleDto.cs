using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.Entities;

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