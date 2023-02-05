using NeKanban.Security.Constants;

namespace NeKanban.Logic.Services.Security;

public interface IPermissionCheckerService
{
    public Task<bool> HasPermission(int deskId, int userId, PermissionType permissionType,  CancellationToken ct);
}