using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Services.Security;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<ToDo>>]
public class ToDoEntityProtector : BaseEntityProtector<ToDo>
{
    private readonly IRepository<ToDo> _toDoRepository;
    public ToDoEntityProtector(IPermissionCheckerService permissionCheckerService,
        IRepository<ToDo> toDoRepository) : base(permissionCheckerService)
    {
        _toDoRepository = toDoRepository;
    }

    protected override async Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        var deskId = await _toDoRepository.FirstOrDefault(x => x.Id == entityId,
            x => x.Column == null ? null : (int?) x.Column.DeskId, ct: ct);
        return deskId;
    }
}