using System.Net;
using JetBrains.Annotations;
using NeKanban.Api.FrameworkExceptions.ExceptionHandling;
using NeKanban.Common.AppMapper;
using NeKanban.Common.Attributes;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.Roles;
using NeKanban.Common.Entities;
using NeKanban.Common.Exceptions;
using NeKanban.Common.Models.RoleModels;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.SecurityProfile;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.Services.Roles;

[UsedImplicitly]
[Injectable<IRolesService>]
public class RolesService : IRolesService
{
    private readonly IRepository<Role> _rolesRepository;
    private readonly IAppMapper _mapper;
    private readonly IRepository<DeskUser> _deskUserRepository;
    private readonly IRepository<RolePermission> _permissionsRepository;
    private readonly IDefaultPermissionProvider _defaultPermissionProvider;

    public RolesService(IRepository<Role> rolesRepository,
        IAppMapper mapper,
        IRepository<DeskUser> deskUserRepository,
        IRepository<RolePermission> permissionsRepository,
        IDefaultPermissionProvider defaultPermissionProvider)
    {
        _rolesRepository = rolesRepository;
        _mapper = mapper;
        _deskUserRepository = deskUserRepository;
        _permissionsRepository = permissionsRepository;
        _defaultPermissionProvider = defaultPermissionProvider;
    }

    public Task<List<RoleWithPermissionsDto>> GetRoles(int deskId, CancellationToken ct)
    {
        return _rolesRepository.ProjectTo<RoleWithPermissionsDto>(x => x.DeskId == deskId, ct);
    }

    public async Task<List<RoleWithPermissionsDto>> CreateRole(int deskId, CreateRoleModel model, CancellationToken ct)
    {
        var role = _mapper.AutoMap<Role, CreateRoleModel>(model);
        role.DeskId = deskId;
        await _rolesRepository.Create(role, ct);
        return await GetRoles(deskId, ct);
    }

    public async Task<List<RoleWithPermissionsDto>> UpdateRole(int roleId, UpdateRoleModel model, CancellationToken ct)
    {
        var role = await _rolesRepository.Single(x => x.Id == roleId, ct);
        _mapper.AutoMap(model, role);
        await _rolesRepository.Update(role, ct);
        return await GetRoles(role.DeskId, ct);
    }

    public async Task<List<RoleWithPermissionsDto>> SetAsDefault(int roleId, CancellationToken ct)
    {
        var role = await _rolesRepository.Single(x => x.Id == roleId, ct);
        role.IsDefault = true;
        var currentDefaultRole =
            await _rolesRepository.ToList(x => x.DeskId == role.DeskId && x.IsDefault, ct);
        foreach (var defaultRole in currentDefaultRole)
        {
            defaultRole.IsDefault = false;
        }
        
        await _rolesRepository.Update(currentDefaultRole, ct);
        await _rolesRepository.Update(role, ct);
        return await GetRoles(role.DeskId, ct);
    }

    public async Task<List<RoleWithPermissionsDto>> DeleteRole(int roleId, CancellationToken ct)
    {
        var anyUsersWithRole = await _deskUserRepository.Any(x => x.RoleId == roleId, ct);
        if (anyUsersWithRole)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest,
                Exceptions.CantDeleteRoleWhenAnyUserHasThisRole);
        }

        var role = await _rolesRepository.Single(x => x.Id == roleId, ct);
        if (role.IsDefault)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, Exceptions.CantDeleteDefaultRole);
        }

        await _rolesRepository.Remove(role, ct);
        return await GetRoles(role.DeskId, ct);
    }

    public Task GrantPermission(int roleId, PermissionType permissionType, CancellationToken ct)
    {
        var permission = new RolePermission
        {
            Permission = permissionType,
            RoleId = roleId
        };

        return _permissionsRepository.Create(permission, ct);
    }

    public async Task RevokePermission(int roleId, PermissionType permissionType, CancellationToken ct)
    {
        var permission = await _permissionsRepository
            .Single(x => x.RoleId == roleId && x.Permission == permissionType, ct);

        await _permissionsRepository.Remove(permission, ct);
    }

    public Task<int?> GetDefaultRoleId(int deskId, CancellationToken ct)
    {
        return _rolesRepository
            .FirstOrDefault(x => x.DeskId == deskId,
                x => (int?) x.Id, ct);
    }

    public async Task CreateDefaultRoles(int deskId, CancellationToken ct)
    {
        await CreateDefaultRole(deskId, DefaultRoleNames.User, true, _defaultPermissionProvider.GetUserPermissions(), ct);
        await CreateDefaultRole(deskId, DefaultRoleNames.Manager, false, _defaultPermissionProvider.GetManagerPermissions(), ct);
    }

    public Task<RoleDto> GetRole(int roleId, CancellationToken ct)
    {
        return _rolesRepository.ProjectToSingle<RoleDto>(x => x.Id == roleId, ct);
    }

    private async Task CreateDefaultRole(int deskId, string roleName, bool isDefault, List<PermissionType> permissionTypes, CancellationToken ct)
    {
        var role = new Role
        {
            Name = roleName,
            DeskId = deskId,
            IsDefault = isDefault
        };

        await _rolesRepository.Create(role, ct);
        var permissions = permissionTypes.Select(x => new RolePermission
        {
            RoleId = role.Id,
            Permission = x
        });

        await _permissionsRepository.Create(permissions, ct);
    }
}