using NeKanban.Security.Constants;

namespace NeKanban.Common.ViewModels;

public class DeskRoleVm
{
    public required RoleType Role { get; set; }
    public string RoleName => Role.ToString();
    public required List<PermissionVm> Permissions { get; set; } = new ();
}