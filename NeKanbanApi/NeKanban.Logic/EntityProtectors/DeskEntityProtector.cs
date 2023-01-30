using JetBrains.Annotations;
using NeKanban.Common.Attributes;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<Desk>>]
public class DeskEntityProtector : BaseEntityProtector<Desk>
{
    public DeskEntityProtector(IRepository<DeskUser> deskUserRepository) : base(deskUserRepository)
    {
    }
    
    protected override Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        return Task.FromResult((int?)entityId);
    }
}