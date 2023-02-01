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
        await EnsureAbleTo<ToDo>(PermissionType.AccessDesk, toDoId, ct);
        return await _commentsService.GetComments(toDoId, ct);
    }
    
    [HttpPost("{toDoId:int}")]
    public async Task<List<CommentDto>> Create(int toDoId, [FromBody]CommentCreateModel model, CancellationToken ct)
    {
        await EnsureAbleTo<ToDo>(PermissionType.AddComments, toDoId, ct);
        return await _commentsService.Create(toDoId, await GetApplicationUser(), model, ct);
    }
    
    [HttpPut("{commentId:int}")]
    public async Task<List<CommentDto>> Update(int commentId, [FromBody]CommentUpdateModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Comment>(PermissionType.UpdateOwnComments, commentId, ct);
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
}
