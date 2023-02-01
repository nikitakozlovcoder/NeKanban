using JetBrains.Annotations;
using NeKanban.Common.Attributes;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<ToDoUser>>]
public class ToDoUserProtector : BaseEntityProtector<ToDoUser>
{
    private readonly IRepository<ToDoUser> _toDoUserRepository;
    public ToDoUserProtector(IRepository<DeskUser> deskUserRepository, IRepository<ToDoUser> toDoUserRepository) : base(deskUserRepository)
    {
        _toDoUserRepository = toDoUserRepository;
    }

    protected override async Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        var deskId = await _toDoUserRepository.FirstOrDefault(x => x.Id == entityId, x => x.DeskUser == null ? null : (int?)x.DeskUser.DeskId, ct: ct);
        return deskId;
    }
}