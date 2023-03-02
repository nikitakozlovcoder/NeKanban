using Batteries.Injection.Attributes;
using JetBrains.Annotations;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.SecurityProfile;

[UsedImplicitly]
[Injectable<IDefaultPermissionProvider>]
public class DefaultPermissionsProvider : IDefaultPermissionProvider
{
    private readonly List<PermissionType> _userPermissions;
    private readonly List<PermissionType> _managerPermissions;
    public DefaultPermissionsProvider()
    {
        _userPermissions = new List<PermissionType>
        {
          PermissionType.AssignTasksThemself,
          PermissionType.MoveTasks,
          PermissionType.AddOrUpdateOwnComments,
          PermissionType.AddOrUpdateOwnComments,
          PermissionType.DeleteOwnComments,
        };
        
        _managerPermissions = new List<PermissionType>
        {
            PermissionType.ViewInviteLink,
            PermissionType.ManageColumns,
            PermissionType.CreateOrUpdateTasks,
            PermissionType.DeleteAnyComments,
        };
        
        _managerPermissions.AddRange(_userPermissions);
    }

    public List<PermissionType> GetUserPermissions() => _userPermissions;

    public List<PermissionType> GetManagerPermissions() => _managerPermissions;
}
