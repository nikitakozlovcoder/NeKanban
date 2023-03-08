using Microsoft.AspNetCore.Http;
using NeKanban.Common.DTOs.Comments;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.CommentModels;

namespace NeKanban.Logic.Services.Comments;

public interface ICommentsService
{
    Task<List<CommentDto>> GetComments(int toDoId, CancellationToken ct);
    Task<CommentDraftDto> GetDraft(int toDoId, ApplicationUser user, CancellationToken ct);
    Task<List<CommentDto>> ApplyDraft(int commentId, ApplicationUser user, CancellationToken ct);
    Task<List<CommentDto>> Update(int commentId, ApplicationUser user, CommentUpdateModel model, CancellationToken ct);
    Task<List<CommentDto>> DeleteOwn(int commentId, ApplicationUser user, CancellationToken ct);
    Task<List<CommentDto>> Delete(int commentId, CancellationToken ct);
    Task<CommentDraftDto> UpdateDraft(int commentId, ApplicationUser getApplicationUser, CommentUpdateModel model, CancellationToken ct);
    Task<string> AttachFile(int commentId, IFormFile file, CancellationToken ct);
}