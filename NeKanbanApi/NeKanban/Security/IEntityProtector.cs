using NeKanban.Constants.Security;
using NeKanban.Data.Entities;

namespace NeKanban.Security;

public interface IEntityProtector<TEntity>
{
    Task<bool> HasPermission(ApplicationUser? currentUser, PermissionType type, int entityId, CancellationToken ct);
}
