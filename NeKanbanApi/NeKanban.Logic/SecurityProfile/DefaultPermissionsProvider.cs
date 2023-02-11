using JetBrains.Annotations;
using NeKanban.Common.Attributes;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.SecurityProfile;

[Injectable<IDefaultPermissionProvider>]
[UsedImplicitly]
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
          PermissionType.AddComments,
          PermissionType.UpdateOwnComments,
          PermissionType.DeleteOwnComments,
        };
        
        _managerPermissions = new List<PermissionType>
        {
            PermissionType.ViewInviteLink,
            PermissionType.ManageColumns,
            PermissionType.CreateTasks,
            PermissionType.DeleteAnyComments,
        };
        
        _managerPermissions.AddRange(_userPermissions);
    }

    public List<PermissionType> GetUserPermissions() => _userPermissions;

    public List<PermissionType> GetManagerPermissions() => _managerPermissions;
}
