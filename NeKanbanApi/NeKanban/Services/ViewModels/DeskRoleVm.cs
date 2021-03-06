using NeKanban.Constants;

namespace NeKanban.Services.ViewModels;

public class DeskRoleVm
{
    public RoleType Role { get; set; }
    public string RoleName => Role.ToString();
    public List<PermissionVm> Permissions { get; set; } = new List<PermissionVm>();
}