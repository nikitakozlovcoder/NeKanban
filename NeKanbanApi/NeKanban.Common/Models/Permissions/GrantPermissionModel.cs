using NeKanban.Security.Constants;

namespace NeKanban.Common.Models.Permissions;

public class GrantPermissionModel
{
    public required PermissionType Permission { get; set; }
}