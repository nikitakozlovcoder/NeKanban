using AutoMapper;
using Batteries.Mapper.AppMapper.Extensions;
using Batteries.Mapper.Interfaces;
using Batteries.Repository;
using NeKanban.Common.Models.CommentModels;

namespace NeKanban.Common.Entities;

public class Comment : IHasPk<int>, IAutoMapFrom<CommentCreateModel, Comment>, IAutoMapFrom<CommentUpdateModel, Comment>
{
    public int Id { get; set; }
    public required int? DeskUserId { get; set; }
    public required int ToDoId { get; set; }
    public required string Body { get; set; } = string.Empty;
    public required DateTime CreatedAtUtc { get; set; }
    public virtual DeskUser? DeskUser { get; set; }
    public virtual ToDo? ToDo { get; set; }
    public static void ConfigureMap(IMappingExpression<CommentCreateModel, Comment> cfg)
    {
        cfg.IgnoreAllMembers()
            .ForMember(x => x.Body, _ => _.MapFrom(x => x.Body));
    }

    public static void ConfigureMap(IMappingExpression<CommentUpdateModel, Comment> cfg)
    {
        cfg.IgnoreAllMembers()
            .ForMember(x => x.Body, _ => _.MapFrom(x => x.Body));
    }
}