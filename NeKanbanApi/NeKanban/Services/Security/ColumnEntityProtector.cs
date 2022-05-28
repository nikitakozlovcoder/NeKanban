using Microsoft.EntityFrameworkCore;
using NeKanban.Constants;
using NeKanban.Data;
using NeKanban.Data.Entities;

namespace NeKanban.Services.Security;

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