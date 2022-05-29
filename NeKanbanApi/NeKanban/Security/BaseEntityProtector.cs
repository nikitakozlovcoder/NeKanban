using Microsoft.EntityFrameworkCore;
using NeKanban.Constants.Security;
using NeKanban.Data;
using NeKanban.Data.Entities;
using NeKanban.Helpers;

namespace NeKanban.Security;

public abstract class BaseEntityProtector<TEntity> : IEntityProtector<TEntity> where TEntity : IHasPk<int>
{
    private readonly IRepository<DeskUser> _deskUserRepository;

    protected BaseEntityProtector(IRepository<DeskUser> deskUserRepository)
    {
        _deskUserRepository = deskUserRepository;
    }

    public async Task<bool> HasPermission(ApplicationUser? currentUser, PermissionType type, int entityId, CancellationToken ct)
    {
        if (currentUser == null)
        {
            return false;
        }

        var id = await GetDeskId(entityId, ct);
        return id.HasValue && await CheckRoleByDeskId(id.Value, currentUser.Id, type, ct);
    }
    
    protected abstract Task<int?> GetDeskId(int entityId, CancellationToken ct);
    
    private async Task<bool> CheckRoleByDeskId(int deskId, int currentUserId, PermissionType type, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.QueryableSelect()
            .FirstOrDefaultAsync(x => x.DeskId == deskId && x.UserId == currentUserId, ct);
        return deskUser != null && PermissionChecker.CheckPermission(deskUser.Role, type);
    }
}