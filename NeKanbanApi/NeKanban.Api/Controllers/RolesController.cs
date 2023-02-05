using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Common.DTOs.Roles;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.Permissions;
using NeKanban.Common.Models.RoleModels;
using NeKanban.Controllers.Auth;
using NeKanban.Logic.Services.Roles;
using NeKanban.Security.Constants;

namespace NeKanban.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class RolesController : BaseAuthController
{
    private readonly IRolesService _rolesService;
    public RolesController(
        UserManager<ApplicationUser> userManager,
        IServiceProvider serviceProvider, IRolesService rolesService) : base(userManager, serviceProvider)
    {
        _rolesService = rolesService;
    }
    
    [HttpGet("{deskId:int}")]
    public async Task<List<RoleWithPermissionsDto>> GetRoles(int deskId, CancellationToken ct)
    {
        await EnsureAbleTo<Desk>(PermissionType.AccessDesk, deskId, ct);
        return await _rolesService.GetRoles(deskId, ct);
    }
    
    [HttpPost("{deskId:int}")]
    public async Task<List<RoleWithPermissionsDto>> CreateRole(int deskId, [FromBody]CreateRoleModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Desk>(PermissionType.ManageRoles, deskId, ct);
        return await _rolesService.CreateRole(deskId, model, ct);
    }
    
    [HttpPut("{roleId:int}")]
    public async Task<List<RoleWithPermissionsDto>> UpdateRole(int roleId, [FromBody]UpdateRoleModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Role>(PermissionType.ManageRoles, roleId, ct);
        return await _rolesService.UpdateRole(roleId, model, ct);
    }
    
    [HttpDelete("{roleId:int}")]
    public async Task<List<RoleWithPermissionsDto>> DeleteRole(int roleId, CancellationToken ct)
    {
        await EnsureAbleTo<Role>(PermissionType.ManageRoles, roleId, ct);
        return await _rolesService.DeleteRole(roleId, ct);
    }
    
    [HttpPost("{roleId:int}")]
    public async Task GrantPermission(int roleId, [FromBody]GrantPermissionModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Role>(PermissionType.ManageRoles, roleId, ct);
        await _rolesService.GrantPermission(roleId, model.Permission, ct);
    }
    
    [HttpPost("{roleId:int}")]
    public async Task RevokePermission(int roleId, [FromBody]RevokePermissionModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Role>(PermissionType.ManageRoles, roleId, ct);
        await _rolesService.GrantPermission(roleId, model.Permission, ct);
    }
}
