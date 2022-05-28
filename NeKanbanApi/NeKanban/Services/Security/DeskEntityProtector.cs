using Microsoft.EntityFrameworkCore;
using NeKanban.Constants;
using NeKanban.Constants.Security;
using NeKanban.Data;
using NeKanban.Data.Entities;
using NeKanban.Helpers;

namespace NeKanban.Services.Security;

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