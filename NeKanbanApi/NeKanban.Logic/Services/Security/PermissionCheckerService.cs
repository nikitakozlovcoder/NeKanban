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
        var deskUser = await _deskUserRepository.ProjectToFirstOrDefault<DeskUserPermissionsChallengeDto>(x => x.DeskId == deskId
            && x.UserId == userId && !x.DeletionReason.HasValue, ct);
        return deskUser != null && HasPermission(deskUser, permission);
    }

    public async Task<bool> HasPermission(int deskId, int userId, CancellationToken ct)
    {
        var validUserExists = await _deskUserRepository.Any(
                x => x.DeskId == deskId && x.UserId == userId && !x.DeletionReason.HasValue, ct);
        return validUserExists;
    }

    private static bool HasPermission(DeskUserPermissionsChallengeDto deskUser, PermissionType permission)
    {
        return deskUser.IsOwner || (deskUser.Permissions != null && deskUser.Permissions.Any(x => x.Permission == permission));
    }
}