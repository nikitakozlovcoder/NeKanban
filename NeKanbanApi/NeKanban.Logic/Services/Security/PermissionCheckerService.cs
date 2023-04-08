using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.Services.Security;

[UsedImplicitly]
[Injectable<IPermissionCheckerService>]
public class PermissionCheckerService : IPermissionCheckerService
{
    private readonly IRepository<DeskUser> _deskUserRepository;

    public PermissionCheckerService(IRepository<DeskUser> deskUserRepository)
    {
        _deskUserRepository = deskUserRepository;
    }

    public async Task<bool> HasPermission(int deskId, int userId, PermissionType permission, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.ProjectToFirstOrDefault<DeskUserPermissionsChallengeDto>(x => x.DeskId == deskId && x.UserId == userId, ct);
        return deskUser != null && HasPermission(deskUser, permission);
    }

    private static bool HasPermission(DeskUserPermissionsChallengeDto deskUser, PermissionType permission)
    {
        return deskUser.IsOwner || (deskUser.Permissions != null && deskUser.Permissions.Any(x => x.Permission == permission));
    }
}