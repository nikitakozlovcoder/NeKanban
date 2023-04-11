using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;
using NeKanban.Logic.Services.Security;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<Desk>>]
public class DeskEntityProtector : BaseEntityProtector<Desk>
{
    public DeskEntityProtector(IPermissionCheckerService permissionCheckerService, 
        IRepository<DeskUser> deskUserRepository) : base(permissionCheckerService, deskUserRepository)
    {
    }
    
    protected override Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        return Task.FromResult((int?)entityId);
    }
}