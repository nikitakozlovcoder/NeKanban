using NeKanban.Constants;
using NeKanban.Constants.Security;

namespace NeKanban.Services.ViewModels;

public class PermissionVm
{
    public PermissionType Permission { get; set; }
    public string PermissionName => Permission.ToString();
}