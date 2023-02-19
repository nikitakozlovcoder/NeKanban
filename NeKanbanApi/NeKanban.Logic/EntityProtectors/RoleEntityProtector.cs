using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Services.Security;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<Role>>]
public class RoleEntityProtector : BaseEntityProtector<Role>
{
    private readonly IRepository<Role> _roleRepository;
    public RoleEntityProtector(IPermissionCheckerService permissionCheckerService,
        IRepository<Role> roleRepository) : base(permissionCheckerService)
    {
        _roleRepository = roleRepository;
    }

    protected override async Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        var deskId = await _roleRepository.SingleOrDefault(x => x.Id == entityId, x => (int?)x.DeskId, ct);
        return deskId;
    }
}