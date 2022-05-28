using NeKanban.Constants.Security;
using NeKanban.Data.Entities;

namespace NeKanban.Services.Security;

public interface IEntityProtector<TEntity>
{
    Task<bool> HasPermission(ApplicationUser? currentUser, PermissionType type, int entityId, CancellationToken ct);
}
