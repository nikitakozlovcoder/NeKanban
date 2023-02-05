using NeKanban.Security.Constants;

namespace NeKanban.Common.ViewModels;

public class ApplicationPermissionVm
{
    public required PermissionType Permission;
    public string PermissionName => Permission.ToString();
}