using NeKanban.Security.Constants;

namespace NeKanban.Logic.SecurityProfile.Helpers;

public static class PermissionChecker
{
    public static bool CheckPermission(RoleType roleType, PermissionType permissionType)
    {
        var deskRoles = new PermissionsRoleMapping().DeskRoles;
        return deskRoles.First(x => x.Role == roleType).Permissions
            .Any(x => x.Permission == permissionType);
    }
}