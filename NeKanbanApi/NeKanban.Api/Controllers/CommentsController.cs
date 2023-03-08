using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Common.DTOs.Comments;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.CommentModels;
using NeKanban.Controllers.Auth;
using NeKanban.Logic.Services.Comments;
using NeKanban.Security.Constants;

namespace NeKanban.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class CommentsController : BaseAuthController
{
    private readonly ICommentsService _commentsService;
    public CommentsController(UserManager<ApplicationUser> userManager,
        IServiceProvider serviceProvider,
        ICommentsService commentsService) : base(userManager, serviceProvider)
    {
        _commentsService = commentsService;
    }
    
    [HttpGet("{toDoId:int}")]
    public async Task<List<CommentDto>> GetComments(int toDoId, CancellationToken ct)
    {
        await EnsureAbleTo<ToDo>(toDoId, ct);
        return await _commentsService.GetComments(toDoId, ct);
    }
    
    [HttpPost("{toDoId:int}")]
    public async Task<CommentDraftDto> GetDraft(int toDoId, CancellationToken ct)
    {
        await EnsureAbleTo<ToDo>(PermissionType.AddOrUpdateOwnComments, toDoId, ct);
        return await _commentsService.GetDraft(toDoId, await GetApplicationUser(), ct);
    }
    
    [HttpPut("{commentId:int}")]
    public async Task<List<CommentDto>> ApplyDraft(int commentId, CancellationToken ct)
    {
        await EnsureAbleTo<Comment>(PermissionType.AddOrUpdateOwnComments, commentId, ct);
        return await _commentsService.ApplyDraft(commentId, await GetApplicationUser(), ct);
    }
    
    [HttpPut("{commentId:int}")]
    public async Task<CommentDraftDto> UpdateDraft(int commentId, [FromBody]CommentUpdateModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Comment>(PermissionType.AddOrUpdateOwnComments, commentId, ct);
        return await _commentsService.UpdateDraft(commentId, await GetApplicationUser(), model, ct);
    }
    
    [HttpPut("{commentId:int}")]
    public async Task<List<CommentDto>> Update(int commentId, [FromBody]CommentUpdateModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Comment>(PermissionType.AddOrUpdateOwnComments, commentId, ct);
        return await _commentsService.Update(commentId, await GetApplicationUser(), model, ct);
    }
    
    [HttpDelete("{commentId:int}")]
    public async Task<List<CommentDto>> DeleteOwn(int commentId, CancellationToken ct)
    {
        await EnsureAbleTo<Comment>(PermissionType.DeleteOwnComments, commentId, ct);
        return await _commentsService.DeleteOwn(commentId, await GetApplicationUser(), ct);
    }
    
    [HttpDelete("{commentId:int}")]
    public async Task<List<CommentDto>> Delete(int commentId, CancellationToken ct)
    {
        await EnsureAbleTo<Comment>(PermissionType.DeleteAnyComments, commentId, ct);
        return await _commentsService.Delete(commentId, ct);
    }
    
    [HttpPut("{commentId:int}")]
    public async Task<string> AttachFile(int commentId, IFormFile file, CancellationToken ct)
    {
        await EnsureAbleTo<Comment>(PermissionType.AddOrUpdateOwnComments, commentId, ct);
        return await _commentsService.AttachFile(commentId, file, ct);
    }
}
