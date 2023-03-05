using Batteries.Injection.Attributes;
using Batteries.Mapper.AppMapper;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.DTOs.Comments;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.CommentModels;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Services.DesksUsers;

namespace NeKanban.Logic.Services.Comments;

[UsedImplicitly]
[Injectable<ICommentsService>]
public class CommentsService : ICommentsService
{
    private readonly IRepository<Comment> _commentsRepository;
    private readonly IRepository<ToDo> _toDosRepository;
    private readonly IAppMapper _appMapper;
    private readonly IDeskUserService _deskUserService;
    private readonly QueryFilterSettings _filterSettings;
    public CommentsService(IRepository<Comment> commentsRepository,
        IRepository<ToDo> toDosRepository,
        IAppMapper appMapper,
        IDeskUserService deskUserService,
        QueryFilterSettings filterSettings)
    {
        _commentsRepository = commentsRepository;
        _toDosRepository = toDosRepository;
        _appMapper = appMapper;
        _deskUserService = deskUserService;
        _filterSettings = filterSettings;
    }

    public Task<List<CommentDto>> GetComments(int toDoId, CancellationToken ct)
    {
        return _commentsRepository.ProjectTo<CommentDto>(x => x.ToDoId == toDoId, ct);
    }

    public async Task<int> CreateDraft(int toDoId, ApplicationUser user, CancellationToken ct)
    {
        var deskId = await _toDosRepository.Single(x => x.Id == toDoId, x => x.Column!.DeskId, ct: ct);
        var deskUserId = await _deskUserService.GetDeskUserId(deskId, user, ct);
        var comment = new Comment
        {
            DeskUserId = deskUserId,
            ToDoId = toDoId,
            Body = string.Empty,
            CreatedAtUtc = default,
            IsDraft = true
        };

        await _commentsRepository.Create(comment, ct);
        return comment.Id;
    }

    public async Task<List<CommentDto>> ApplyDraft(int commentId, ApplicationUser user, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            CommentDraftFilter = false
        });
        
        var comment = await _commentsRepository.Single(x => x.IsDraft && x.DeskUser != null
                                                                && x.DeskUser.UserId == user.Id, ct);

        comment.CreatedAtUtc = DateTime.UtcNow;
        comment.IsDraft = false;
        await _commentsRepository.Update(comment, ct);
        return await GetComments(comment.ToDoId, ct);
    }

    public async Task<List<CommentDto>> Update(bool isDraft, int commentId, ApplicationUser user, CommentUpdateModel model, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            CommentDraftFilter = false
        });

        var comment = await _commentsRepository
            .Single(x => x.Id == commentId
                         && x.DeskUser != null
                         && x.DeskUser.UserId == user.Id
                         && x.IsDraft == isDraft, ct);
        
        _appMapper.AutoMap(model, comment);
        await _commentsRepository.Update(comment, ct);
        return await GetComments(comment.ToDoId, ct);
    }

    public async Task<List<CommentDto>> DeleteOwn(int commentId, ApplicationUser user, CancellationToken ct)
    {
        var comment = await _commentsRepository.Single(x =>
                x.Id == commentId && x.DeskUser != null && x.DeskUser.UserId == user.Id, ct);
        return await Delete(comment, ct);
    }

    public async Task<List<CommentDto>> Delete(int commentId, CancellationToken ct)
    {
        var comment = await _commentsRepository.Single(x => x.Id == commentId, ct);
        return await Delete(comment, ct);
    }
    
    private async Task<List<CommentDto>> Delete(Comment comment, CancellationToken ct)
    {
        await _commentsRepository.Remove(comment, ct);
        return await GetComments(comment.ToDoId, ct);
    }
}