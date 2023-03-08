using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Services.Security;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<DeskUser>>]
public class DeskUserEntityProtector : BaseEntityProtector<DeskUser>
{
    private readonly IRepository<DeskUser> _deskUserRepository;
    public DeskUserEntityProtector(IRepository<DeskUser> deskUserRepository,
        IPermissionCheckerService permissionCheckerService) : base(permissionCheckerService)
    {
        _deskUserRepository = deskUserRepository;
    }

    protected override async Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.FirstOrDefault(x => x.Id == entityId, ct);
        return deskUser?.DeskId;
    }
}