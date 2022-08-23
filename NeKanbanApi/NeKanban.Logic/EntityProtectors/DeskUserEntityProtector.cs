using Microsoft.EntityFrameworkCore;
using NeKanban.Data.Entities;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.EntityProtectors;

public class DeskUserEntityProtector : BaseEntityProtector<DeskUser>
{
    private readonly IRepository<DeskUser> _deskUserRepository;
    public DeskUserEntityProtector(IRepository<DeskUser> deskUserRepository) : base(deskUserRepository)
    {
        _deskUserRepository = deskUserRepository;
    }

    protected override async Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.QueryableSelect()
            .FirstOrDefaultAsync(x => x.Id == entityId, ct);
        return deskUser?.DeskId;
    }
}