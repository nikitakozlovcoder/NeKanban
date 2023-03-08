using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;

namespace NeKanban.Common.DTOs.Comments;

public class CommentDto : BaseEntityModel<int>, IAutoMapFrom<Comment, CommentDto>
{
    public required string Body { get; set; }
    public required DateTime CreatedAtUtc { get; set; }
    public required DeskUserLiteDto? DeskUser { get; set; }
    public static void ConfigureMap(IMappingExpression<Comment, CommentDto> cfg)
    {
    }
}