using NeKanban.Common.DTOs.Comments;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.CommentModels;

namespace NeKanban.Logic.Services.Comments;

public interface ICommentsService
{
    Task<List<CommentDto>> GetComments(int taskId, CancellationToken ct);
    Task<List<CommentDto>> Create(int toDoId, ApplicationUser user, CommentCreateModel model, CancellationToken ct);
    Task<List<CommentDto>> Update(int commentId, ApplicationUser user, CommentUpdateModel model, CancellationToken ct);
    Task<List<CommentDto>> DeleteOwn(int commentId, ApplicationUser user, CancellationToken ct);
    Task<List<CommentDto>> Delete(int commentId, CancellationToken ct);
}