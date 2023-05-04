using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure.QueryFilters;
using NeKanban.Logic.Services.Security;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<Comment>>]
public class CommentEntityProtector : BaseEntityProtector<Comment>
{
    private readonly IRepository<Comment> _commentsRepository;
    private readonly QueryFilterSettings _filterSettings;
    public CommentEntityProtector(IPermissionCheckerService permissionCheckerService,
        IRepository<Comment> commentsRepository,
        QueryFilterSettings filterSettings) : base(permissionCheckerService)
    {
        _commentsRepository = commentsRepository;
        _filterSettings = filterSettings;
    }

    protected override Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            CommentDraftFilter = false
        });
        
        return _commentsRepository.Single(x => x.Id == entityId, x => (int?)x.ToDo!.Column!.DeskId, ct);
    }
}