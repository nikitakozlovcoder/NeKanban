using NeKanban.Common.Entities;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.EntityProtectors;

public interface IEntityProtector<TEntity>
{
    Task<bool> HasPermission(ApplicationUser? currentUser, PermissionType type, int entityId, CancellationToken ct);
}
