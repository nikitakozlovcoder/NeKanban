using NeKanban.Security.Constants;

namespace NeKanban.Logic.SecurityProfile;

public interface IDefaultPermissionProvider
{
    List<PermissionType> GetUserPermissions();
    List<PermissionType> GetManagerPermissions();
}