using Microsoft.EntityFrameworkCore;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.SecurityProfile.Helpers;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.EntityProtectors;

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
        var deskUser = await _deskUserRepository.FirstOrDefault(x => x.DeskId == deskId && x.UserId == currentUserId,
            x => new
            {
                x.Role
            }, ct: ct);
        
        return deskUser != null && PermissionChecker.CheckPermission(deskUser.Role, type);
    }
}