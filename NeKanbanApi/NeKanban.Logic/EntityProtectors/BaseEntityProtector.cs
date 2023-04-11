using Batteries.Repository;
using NeKanban.Common.Entities;
using NeKanban.Logic.Services.Security;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.EntityProtectors;

public abstract class BaseEntityProtector<TEntity> : IEntityProtector<TEntity> where TEntity : IHasPk<int>
{
    private readonly IPermissionCheckerService _permissionCheckerService;
    private readonly IRepository<DeskUser> _deskUserRepository;
    protected BaseEntityProtector (IPermissionCheckerService permissionCheckerService, IRepository<DeskUser> deskUserRepository)
    {
        _permissionCheckerService = permissionCheckerService;
        _deskUserRepository = deskUserRepository;
    }

    public async Task<bool> HasPermission(ApplicationUser? currentUser, PermissionType type, int entityId, CancellationToken ct)
    {
        if (currentUser == null)
        {
            return false;
        }

        var id = await GetDeskId(entityId, ct);
        return id.HasValue && await CheckRoleByDeskId(id.Value, currentUser.Id, type, ct) && await CheckDeskUserNotDeleted(currentUser, id.Value, ct);
    }

    public async Task<bool> HasPermission(ApplicationUser? currentUser, int entityId, CancellationToken ct)
    {
        if (currentUser == null)
        {
            return false;
        }

        var id = await GetDeskId(entityId, ct);
        return id.HasValue && await CheckDeskUserNotDeleted(currentUser, id.Value, ct);
    }

    protected abstract Task<int?> GetDeskId(int entityId, CancellationToken ct);
    
    private async Task<bool> CheckRoleByDeskId(int deskId, int currentUserId, PermissionType type, CancellationToken ct)
    {
        return await _permissionCheckerService.HasPermission(deskId, currentUserId, type, ct);
    }

    private Task<bool> CheckDeskUserNotDeleted(ApplicationUser currentUser, int deskId, CancellationToken ct)
    {
        return _deskUserRepository.Any(x => x.UserId == currentUser.Id && !x.DeletionReason.HasValue && x.DeskId == deskId, ct);
    }
}