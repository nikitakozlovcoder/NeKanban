using Batteries.Mapper.Interfaces;
using NeKanban.Common.ViewModels.Roles;

namespace NeKanban.Common.DTOs.Roles;

public class RoleVm : BaseEntityModel<int>, IMapFrom<RoleDto, RoleVm>
{
    public required string Name { get; set; }
    public required bool IsDefault { get; set; }
    public required int DeskId { get; set; }
}