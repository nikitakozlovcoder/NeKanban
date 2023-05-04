using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;
using NeKanban.Logic.Services.Security;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<Desk>>]
public class DeskEntityProtector : BaseEntityProtector<Desk>
{
    private readonly IRepository<Desk> _deskRepository;
    public DeskEntityProtector(IPermissionCheckerService permissionCheckerService,
        IRepository<Desk> deskRepository) : base(permissionCheckerService)
    {
        _deskRepository = deskRepository;
    }
    
    protected override async Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        await _deskRepository.AnyOrThrow(x => x.Id == entityId, ct);
        return entityId;
    }
}