using NeKanban.Data;
using NeKanban.Data.Entities;

namespace NeKanban.Security;

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