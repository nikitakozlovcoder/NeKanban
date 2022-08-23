using Microsoft.EntityFrameworkCore;
using NeKanban.Data.Entities;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.EntityProtectors;

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