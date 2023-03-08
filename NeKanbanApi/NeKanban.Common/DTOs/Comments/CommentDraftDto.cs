using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.Entities;

namespace NeKanban.Common.DTOs.Comments;

public class CommentDraftDto : BaseEntityModel<int>, IAutoMapFrom<Comment, CommentDraftDto>
{
    public required string Body { get; set; }
    public static void ConfigureMap(IMappingExpression<Comment, CommentDraftDto> cfg)
    {
    }
}