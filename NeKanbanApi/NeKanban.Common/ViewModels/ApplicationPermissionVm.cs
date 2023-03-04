using NeKanban.Security.Constants;

namespace NeKanban.Common.ViewModels;

public class ApplicationPermissionVm
{
    public required PermissionType Permission { get; set; }
    public string PermissionName => Permission.ToString();
}