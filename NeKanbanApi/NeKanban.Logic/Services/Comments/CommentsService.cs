using Batteries.FileStorage.FileStorageAdapters;
using Batteries.FileStorage.FileStorageProxies;
using Batteries.Injection.Attributes;
using Batteries.Mapper.AppMapper;
using Batteries.Repository;
using Batteries.Validation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NeKanban.Common.DTOs.Comments;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.CommentModels;
using NeKanban.Data.Infrastructure;
using NeKanban.Data.Infrastructure.QueryFilters;
using NeKanban.Logic.Services.DesksUsers;
using NeKanban.Logic.ValidationProfiles.Comments;

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
    private readonly IFileStorageAdapter<CommentFileAdapter, Comment> _fileStorageAdapter;
    private readonly IFileStorageProxy _fileStorageProxy;
    private readonly IAppValidator<CommentValidationModel> _commentValidator;
    public CommentsService(IRepository<Comment> commentsRepository,
        IRepository<ToDo> toDosRepository,
        IAppMapper appMapper,
        IDeskUserService deskUserService,
        QueryFilterSettings filterSettings,
        IFileStorageAdapter<CommentFileAdapter, Comment> fileStorageAdapter,
        IFileStorageProxy fileStorageProxy,
        IAppValidator<CommentValidationModel> commentValidator)
    {
        _commentsRepository = commentsRepository;
        _toDosRepository = toDosRepository;
        _appMapper = appMapper;
        _deskUserService = deskUserService;
        _filterSettings = filterSettings;
        _fileStorageAdapter = fileStorageAdapter;
        _fileStorageProxy = fileStorageProxy;
        _commentValidator = commentValidator;
    }

    public Task<List<CommentDto>> GetComments(int toDoId, CancellationToken ct)
    {
        return _commentsRepository.ProjectTo<CommentDto>(x => x.ToDoId == toDoId, ct);
    }

    public async Task<CommentDraftDto> GetDraft(int toDoId, ApplicationUser user, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions()
        {
            CommentDraftFilter = false
        });
        
        var draft = await _commentsRepository.ProjectToFirstOrDefault<CommentDraftDto>(x =>
            x.IsDraft && x.DeskUser != null && x.DeskUser.UserId == user.Id && x.ToDoId == toDoId, ct);

        if (draft != null)
        {
            return draft;
        }
        
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
        return _appMapper.AutoMap<CommentDraftDto, Comment>(comment);
    }

    public async Task<List<CommentDto>> ApplyDraft(int commentId, ApplicationUser user, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            CommentDraftFilter = false
        });
        
        var comment = await _commentsRepository.Single(x => x.IsDraft && x.DeskUser != null
                                                                && x.DeskUser.UserId == user.Id && x.Id == commentId, ct);
        await _commentValidator.ValidateOrThrow(new CommentValidationModel
        {
            Body = comment.Body
        }, ct);
        
        comment.CreatedAtUtc = DateTime.UtcNow;
        comment.IsDraft = false;
        await _commentsRepository.Update(comment, ct);
        return await GetComments(comment.ToDoId, ct);
    }

    public async Task<List<CommentDto>> Update(int commentId, ApplicationUser user, CommentUpdateModel model, CancellationToken ct)
    {
        await _commentValidator.ValidateOrThrow(new CommentValidationModel
        {
            Body = model.Body
        }, ct);
        
        var comment = await _commentsRepository
            .Single(x => x.Id == commentId
                         && x.DeskUser != null
                         && x.DeskUser.UserId == user.Id, ct);
        _appMapper.AutoMap(model, comment);
        await _commentsRepository.Update(comment, ct);
        return await GetComments(comment.ToDoId, ct);
    }

    public async Task<CommentDraftDto> UpdateDraft(int commentId, ApplicationUser user, CommentUpdateModel model, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            CommentDraftFilter = false
        });

        var comment = await _commentsRepository
            .Single(x => x.Id == commentId
                         && x.DeskUser != null
                         && x.DeskUser.UserId == user.Id
                         && x.IsDraft, ct);
        
        _appMapper.AutoMap(model, comment);
        await _commentsRepository.Update(comment, ct);
        return _appMapper.AutoMap<CommentDraftDto, Comment>(comment);
    }

    public async Task<string> AttachFile(int commentId, IFormFile file, CancellationToken ct)
    {
        var result = await _fileStorageAdapter.Store(commentId, file, ct);
        return _fileStorageProxy.GetProxyUrl(result.FileName);
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
    
    private Task<CommentDto> GetComment(int id, CancellationToken ct)
    {
        return _commentsRepository.ProjectToSingle<CommentDto>(x => x.Id == id, ct);
    }
}