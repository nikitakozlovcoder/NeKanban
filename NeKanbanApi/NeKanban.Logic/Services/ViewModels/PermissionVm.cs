using NeKanban.Security.Constants;

namespace NeKanban.Logic.Services.ViewModels;

public class PermissionVm
{
    public PermissionType Permission { get; set; }
    public string PermissionName => Permission.ToString();
}