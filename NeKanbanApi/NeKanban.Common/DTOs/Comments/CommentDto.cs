using AutoMapper;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.DTOs.Comments;

public class CommentDto : BaseEntityDto<int>, IMapFrom<Comment, CommentDto>
{
    public required string Body { get; set; } = null!;
    public required DateTime CreatedAtUtc { get; set; }
    public required DeskUserLiteDto? DeskUser { get; set; }
    public static void ConfigureMap(IMappingExpression<Comment, CommentDto> cfg)
    {
    }
}