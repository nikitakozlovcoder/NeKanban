using Batteries.Injection.Attributes;
using Batteries.Mapper.AppMapper;
using Batteries.Repository;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NeKanban.Common;
using NeKanban.Common.Entities;

namespace NeKanban.Data.Repositories;

[UsedImplicitly]
[Injectable<IRepository<DeskUser>>]
public class DeskUserRepository : Repository<DeskUser>
{
    public DeskUserRepository(DbContext context, IAppMapper mapper) : base(context, mapper)
    {
    }

    public override Task Remove(DeskUser item, CancellationToken ct)
    {
        item.DeletionReason = DeskUserDeletionReason.Removed;
        return base.Update(item, ct);
    }

    public override Task Remove(IEnumerable<DeskUser> items, CancellationToken ct)
    {
        var listItems = items.ToList();
        foreach (var item in listItems)
        {
            item.DeletionReason = DeskUserDeletionReason.Removed;
        }
        
        return base.Update(listItems, ct);
    }
}