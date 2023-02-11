using JetBrains.Annotations;
using NeKanban.Common.Entities;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.EntityProtectors;

public interface IEntityProtector<[UsedImplicitly]TEntity>
{
    Task<bool> HasPermission(ApplicationUser? currentUser, PermissionType type, int entityId, CancellationToken ct);
    Task<bool> HasPermission(ApplicationUser? currentUser, int entityId, CancellationToken ct);
}
