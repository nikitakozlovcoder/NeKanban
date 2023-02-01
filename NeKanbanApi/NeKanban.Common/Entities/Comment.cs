using AutoMapper;
using NeKanban.Common.Extensions;
using NeKanban.Common.Interfaces;
using NeKanban.Common.Models.CommentModels;

namespace NeKanban.Common.Entities;

public class Comment : IHasPk<int>, IMapFrom<CommentCreateModel, Comment>, IMapFrom<CommentUpdateModel, Comment>
{
    public int Id { get; set; }
    public int? DeskUserId { get; set; }
    public int ToDoId { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
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