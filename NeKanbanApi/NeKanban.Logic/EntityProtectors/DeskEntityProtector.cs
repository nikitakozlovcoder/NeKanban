using Batteries.Injection.Attributes;
using JetBrains.Annotations;
using NeKanban.Common.Entities;
using NeKanban.Logic.Services.Security;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<Desk>>]
public class DeskEntityProtector : BaseEntityProtector<Desk>
{
    public DeskEntityProtector(IPermissionCheckerService permissionCheckerService) : base(permissionCheckerService)
    {
    }
    
    protected override Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        return Task.FromResult((int?)entityId);
    }
}