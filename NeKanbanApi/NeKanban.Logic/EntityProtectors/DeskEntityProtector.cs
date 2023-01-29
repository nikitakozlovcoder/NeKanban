using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.EntityProtectors;

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