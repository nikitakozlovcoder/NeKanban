using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Services.Security;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<Column>>]
public class ColumnEntityProtector : BaseEntityProtector<Column>
{
    private readonly IRepository<Column> _columnRepository;
    public ColumnEntityProtector(IPermissionCheckerService permissionCheckerService,
        IRepository<Column> columnRepository) : base(permissionCheckerService)
    {
        _columnRepository = columnRepository;
    }
    
    protected override async Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        var column = await _columnRepository.FirstOrDefault(x => x.Id == entityId, ct);
        return column?.DeskId;
    }
}