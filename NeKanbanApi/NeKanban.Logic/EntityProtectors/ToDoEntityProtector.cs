using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure.QueryFilters;
using NeKanban.Logic.Services.Security;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<ToDo>>]
public class ToDoEntityProtector : BaseEntityProtector<ToDo>
{
    private readonly IRepository<ToDo> _toDoRepository;
    private readonly QueryFilterSettings _filterSettings;
    public ToDoEntityProtector(IPermissionCheckerService permissionCheckerService,
        IRepository<ToDo> toDoRepository, QueryFilterSettings filterSettings, 
        IRepository<DeskUser> deskUserRepository) : base(permissionCheckerService, deskUserRepository)
    {
        _toDoRepository = toDoRepository;
        _filterSettings = filterSettings;
    }

    protected override async Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            ToDoDraftFilter = false
        });
        
        var deskId = await _toDoRepository.First(x => x.Id == entityId,
            x => x.Column == null ? null : (int?) x.Column.DeskId, ct: ct);
        return deskId;
    }
}