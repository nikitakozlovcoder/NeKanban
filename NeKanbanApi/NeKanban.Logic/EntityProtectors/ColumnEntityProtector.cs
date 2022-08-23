using Microsoft.EntityFrameworkCore;
using NeKanban.Data.Entities;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.EntityProtectors;

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