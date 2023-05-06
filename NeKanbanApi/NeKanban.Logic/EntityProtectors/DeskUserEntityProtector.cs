using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure.QueryFilters;
using NeKanban.Logic.Services.Security;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<DeskUser>>]
public class DeskUserEntityProtector : BaseEntityProtector<DeskUser>
{
    private readonly IRepository<DeskUser> _deskUserRepository;
    private readonly QueryFilterSettings _filterSettings;
    public DeskUserEntityProtector(IRepository<DeskUser> deskUserRepository,
        IPermissionCheckerService permissionCheckerService, QueryFilterSettings filterSettings) : base(permissionCheckerService)
    {
        _deskUserRepository = deskUserRepository;
        _filterSettings = filterSettings;
    }

    protected override async Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            DeskUserDeletedFilter = false
        });
        var deskUser = await _deskUserRepository.First(x => x.Id == entityId, ct);
        return deskUser.DeskId;
    }
}