using NeKanban.Common.ViewModels;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.SecurityProfile;

public class PermissionsRoleMapping
{
    public List<DeskRoleVm> DeskRoles { get; } = new ();

    public PermissionsRoleMapping()
    {
        var userPermissions = new List<PermissionVm>
        {
            new ()
            {
                Permission = PermissionType.AccessDesk
            },
            new()
            {
                Permission = PermissionType.AssignTasksThemself
            },
            new()
            {
                Permission = PermissionType.MoveTasks
            },
            new()
            {
                Permission = PermissionType.AddComments
            },
            new()
            {
                Permission = PermissionType.UpdateOwnComments
            },
            new()
            {
                Permission = PermissionType.DeleteOwnComments
            }
        };
        
        var managerPermissions = new List<PermissionVm>
        {
            new()
            {
                Permission = PermissionType.ManageAssigners
            },
            new()
            {
                Permission = PermissionType.ViewInviteLink
            },
            new()
            {
                Permission = PermissionType.CreateColumns
            },
            new()
            {
                Permission = PermissionType.ManageColumns
            },
            new ()
            {
                Permission = PermissionType.CreateTasks
            },
            new()
            {
                Permission = PermissionType.UpdateTask
            },
            new()
            {
                Permission = PermissionType.DeleteAnyComments
            }
        };
        
        managerPermissions.AddRange(userPermissions);
        var ownerPermissions = new List<PermissionVm>
        {
            new()
            {
                Permission = PermissionType.ManageInviteLink
            },
            new()
            {
                Permission = PermissionType.DeleteDesk
            },
            new()
            {
                Permission = PermissionType.UpdateGeneralDesk
            },
            new()
            {
                Permission = PermissionType.RemoveUsers
            },
            new()
            {
                Permission = PermissionType.ChangeUserRole
            }
        };
        
        ownerPermissions.AddRange(managerPermissions);
        DeskRoles.Add(new DeskRoleVm
        {
            Role = RoleType.User,
            Permissions = userPermissions
        });
        
        DeskRoles.Add(new DeskRoleVm
        {
            Role = RoleType.Manager,
            Permissions = managerPermissions
        });
        
        DeskRoles.Add(new DeskRoleVm
        {
            Role = RoleType.Owner,
            Permissions = ownerPermissions
        });
    }
}
