using NeKanban.Constants;
using NeKanban.Constants.Security;

namespace NeKanban.Helpers;

public static class PermissionChecker
{
    public static bool CheckPermission(RoleType roleType, PermissionType permissionType)
    {
        var deskRoles = new PermissionsRoleMapping().DeskRoles;
        return deskRoles.First(x => x.Role == roleType).Permissions
            .Any(x => x.Permission == permissionType);
    }
}