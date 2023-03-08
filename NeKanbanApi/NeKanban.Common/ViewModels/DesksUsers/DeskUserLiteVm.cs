using Batteries.Mapper.Interfaces;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.DTOs.Roles;
using NeKanban.Common.ViewModels.ApplicationUsers;

namespace NeKanban.Common.ViewModels.DesksUsers;

public class DeskUserLiteVm : BaseEntityModel<int>, IMapFrom<DeskUserLiteDto, DeskUserLiteVm>
{
    public required int UserId { get; set; }
    public required ApplicationUserVm User { get; set; }
    public required bool IsOwner { get; set; }
    public required RoleVm? Role { get; set; }
}