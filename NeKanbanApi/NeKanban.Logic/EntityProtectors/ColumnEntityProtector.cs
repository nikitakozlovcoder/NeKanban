using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NeKanban.Common.Attributes;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<Column>>]
public class ColumnEntityProtector : BaseEntityProtector<Column>
{
    private readonly IRepository<Column> _columnRepository;
    public ColumnEntityProtector(IRepository<DeskUser> deskUserRepository,
        IRepository<Column> columnRepository) : base(deskUserRepository)
    {
        _columnRepository = columnRepository;
    }
    
    protected override async Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        var column = await _columnRepository.QueryableSelect().FirstOrDefaultAsync(x => x.Id == entityId, ct);
        return column?.DeskId;
    }
}