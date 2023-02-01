using JetBrains.Annotations;
using NeKanban.Common.Attributes;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<Comment>>]
public class CommentEntityProtector : BaseEntityProtector<Comment>
{
    private readonly IRepository<Comment> _commentsRepository;
    public CommentEntityProtector(IRepository<DeskUser> deskUserRepository,
        IRepository<Comment> commentsRepository) : base(deskUserRepository)
    {
        _commentsRepository = commentsRepository;
    }

    protected override Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        return _commentsRepository.Single(x => x.Id == entityId, x => (int?)x.ToDo!.Column!.DeskId, ct);
    }
}