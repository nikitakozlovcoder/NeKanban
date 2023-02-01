using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NeKanban.Common.Attributes;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<ToDo>>]
public class ToDoEntityProtector : BaseEntityProtector<ToDo>
{
    private readonly IRepository<ToDo> _toDoRepository;
    public ToDoEntityProtector(IRepository<DeskUser> deskUserRepository,
        IRepository<ToDo> toDoRepository) : base(deskUserRepository)
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