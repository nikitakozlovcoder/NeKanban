using NeKanban.Common.DTOs.Comments;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.CommentModels;

namespace NeKanban.Logic.Services.Comments;

public interface ICommentsService
{
    Task<List<CommentDto>> GetComments(int toDoId, CancellationToken ct);
    Task<int> CreateDraft(int toDoId, ApplicationUser user, CancellationToken ct);
    Task<List<CommentDto>> ApplyDraft(int commentId, ApplicationUser user, CancellationToken ct);
    Task<List<CommentDto>> Update(bool isDraft, int commentId, ApplicationUser user, CommentUpdateModel model, CancellationToken ct);
    Task<List<CommentDto>> DeleteOwn(int commentId, ApplicationUser user, CancellationToken ct);
    Task<List<CommentDto>> Delete(int commentId, CancellationToken ct);
}