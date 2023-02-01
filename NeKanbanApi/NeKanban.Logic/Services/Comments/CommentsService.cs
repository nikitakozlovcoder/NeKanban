using JetBrains.Annotations;
using NeKanban.Common.AppMapper;
using NeKanban.Common.Attributes;
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
    public CommentsService(IRepository<Comment> commentsRepository,
        IRepository<ToDo> toDosRepository,
        IAppMapper appMapper,
        IDeskUserService deskUserService)
    {
        _commentsRepository = commentsRepository;
        _toDosRepository = toDosRepository;
        _appMapper = appMapper;
        _deskUserService = deskUserService;
    }

    public Task<List<CommentDto>> GetComments(int toDoId, CancellationToken ct)
    {
        return _commentsRepository.ProjectTo<CommentDto>(x => x.ToDoId == toDoId, ct);
    }

    public async Task<List<CommentDto>> Create(int toDoId, ApplicationUser user, CommentCreateModel model, CancellationToken ct)
    {
        var deskId = await _toDosRepository.Single(x => x.Id == toDoId, x => x.Column!.DeskId, ct: ct);
        var deskUserId = await _deskUserService.GetDeskUserId(deskId, user, ct);
        var comment = _appMapper.Map<Comment, CommentCreateModel>(model);
        comment.DeskUserId = deskUserId;
        comment.ToDoId = toDoId;
        comment.CreatedAtUtc = DateTime.UtcNow;
        await _commentsRepository.Create(comment, ct);
        return await GetComments(toDoId, ct);
    }

    public async Task<List<CommentDto>> Update(int commentId, ApplicationUser user, CommentUpdateModel model, CancellationToken ct)
    {
        var comment = await _commentsRepository
            .Single(x => x.Id == commentId && x.DeskUser != null && x.DeskUser.UserId == user.Id, ct);

        _appMapper.Map(model, comment);
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