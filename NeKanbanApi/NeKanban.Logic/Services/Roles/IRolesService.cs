using NeKanban.Common.DTOs.Roles;
using NeKanban.Common.Models.RoleModels;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.Services.Roles;

public interface IRolesService
{
    Task<List<RoleWithPermissionsDto>> GetRoles(int deskId, CancellationToken ct);
    Task<List<RoleWithPermissionsDto>> CreateRole(int deskId, CreateRoleModel model, CancellationToken ct);
    Task<List<RoleWithPermissionsDto>> UpdateRole(int roleId, UpdateRoleModel model, CancellationToken ct);
    Task<List<RoleWithPermissionsDto>> DeleteRole(int roleId, CancellationToken ct);
    Task GrantPermission(int roleId, PermissionType permission, CancellationToken ct);
    Task RevokePermission(int roleId, PermissionType permission, CancellationToken ct);
    Task<int?> GetDefaultRoleId(int deskId, CancellationToken ct);
    Task CreateDefaultRoles(int deskId, CancellationToken ct);
}