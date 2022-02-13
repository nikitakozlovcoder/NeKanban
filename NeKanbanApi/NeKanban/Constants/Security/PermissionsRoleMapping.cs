﻿using NeKanban.Services.ViewModels;

namespace NeKanban.Constants.Security;

public class PermissionsRoleMapping
{
    public List<DeskRoleVm> DeskRoles { get; set; } = new List<DeskRoleVm>();

    public PermissionsRoleMapping()
    {
        var userPermissions = new List<PermissionVm>
        {
            new()
            {
                Permission = PermissionType.AssignTasksThemself
            },
            new()
            {
                Permission = PermissionType.MoveTasks
            }
        };
        
       var managerPermissions = new List<PermissionVm>
        {
            new()
            {
                Permission = PermissionType.AssignTasks
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
            new()
            {
                Permission = PermissionType.UpdateTask
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
            }
        };
        ownerPermissions.AddRange(managerPermissions);
        
        DeskRoles.Add(new DeskRoleVm()
        {
            Role = RoleType.User,
            Permissions = userPermissions
        });
        
        DeskRoles.Add(new DeskRoleVm()
        {
            Role = RoleType.Manager,
            Permissions = managerPermissions
        });
        
        DeskRoles.Add(new DeskRoleVm()
        {
            Role = RoleType.Owner,
            Permissions = ownerPermissions
        });
    }
}