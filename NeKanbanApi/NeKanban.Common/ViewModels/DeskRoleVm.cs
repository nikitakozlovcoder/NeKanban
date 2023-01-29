using NeKanban.Security.Constants;

namespace NeKanban.Common.ViewModels;

public class DeskRoleVm
{
    public RoleType Role { get; set; }
    public string RoleName => Role.ToString();
    public List<PermissionVm> Permissions { get; set; } = new ();
}