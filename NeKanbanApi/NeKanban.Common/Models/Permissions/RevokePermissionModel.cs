using NeKanban.Security.Constants;

namespace NeKanban.Common.Models.Permissions;

public class RevokePermissionModel
{
    public required PermissionType Permission { get; set; }
}