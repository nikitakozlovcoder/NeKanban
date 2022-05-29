using Microsoft.EntityFrameworkCore;
using NeKanban.Data;
using NeKanban.Data.Entities;

namespace NeKanban.Security;

public class ToDoUserProtector : BaseEntityProtector<ToDoUser>
{
    private readonly IRepository<ToDoUser> _toDoUserRepository;
    public ToDoUserProtector(IRepository<DeskUser> deskUserRepository, IRepository<ToDoUser> toDoUserRepository) : base(deskUserRepository)
    {
        _toDoUserRepository = toDoUserRepository;
    }

    protected override async Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        var todoUser = await _toDoUserRepository.QueryableSelect().Include(x => x.DeskUser)
            .FirstOrDefaultAsync(x => x.Id == entityId, ct);
        return todoUser?.DeskUser?.DeskId;
    }
}