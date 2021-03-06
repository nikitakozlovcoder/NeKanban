using Microsoft.EntityFrameworkCore;
using NeKanban.Data;
using NeKanban.Data.Entities;

namespace NeKanban.Security;

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
        var todo = await _toDoRepository.QueryableSelect().Include(x => x.Column)
            .FirstOrDefaultAsync(x => x.Id == entityId, ct);
        return todo?.Column?.DeskId;
    }
}