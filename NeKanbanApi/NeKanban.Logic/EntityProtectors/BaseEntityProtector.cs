using Batteries.Repository;
using NeKanban.Common.Entities;
using NeKanban.Logic.Services.Security;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.EntityProtectors;

public abstract class BaseEntityProtector<TEntity> : IEntityProtector<TEntity> where TEntity : IHasPk<int>
{
    private readonly IPermissionCheckerService _permissionCheckerService;
    protected BaseEntityProtector (IPermissionCheckerService permissionCheckerService)
    {
        _permissionCheckerService = permissionCheckerService;
    }

    public async Task<bool> HasPermission(ApplicationUser? currentUser, PermissionType type, int entityId, CancellationToken ct)
    {
        if (currentUser == null)
        {
            return false;
        }

        var id = await GetDeskId(entityId, ct);
        return id.HasValue && await CheckUserHasPermission(id.Value, currentUser.Id, type, ct);
    }

    public async Task<bool> HasPermission(ApplicationUser? currentUser, int entityId, CancellationToken ct)
    {
        if (currentUser == null)
        {
            return false;
        }

        var id = await GetDeskId(entityId, ct);
        return id.HasValue && await CheckUserHasPermission(id.Value, currentUser.Id, ct);
    }

    protected abstract Task<int?> GetDeskId(int entityId, CancellationToken ct);
    
    private Task<bool> CheckUserHasPermission(int deskId, int currentUserId, PermissionType type, CancellationToken ct)
    {
        return _permissionCheckerService.HasPermission(deskId, currentUserId, type, ct);
    }

    private Task<bool> CheckUserHasPermission(int deskId, int currentUserId, CancellationToken ct)
    {
        return _permissionCheckerService.HasPermission(deskId, currentUserId, ct);
    }
}