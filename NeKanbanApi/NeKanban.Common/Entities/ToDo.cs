﻿using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using NeKanban.Common.Extensions;
using NeKanban.Common.Interfaces;
using NeKanban.Common.Models.ToDoModels;

namespace NeKanban.Common.Entities;

public class ToDo: IHasPk<int>, IAutoMapFrom<ToDoCreateModel, ToDo>, IAutoMapFrom<ToDoUpdateModel, ToDo>
{
    public int Id { get; set; }
    public required int Order { get; set; }
    public required string Name { get; set; }
    public required string? Body { get; set; }
    public required int ColumnId { get; set; }
    public virtual Column? Column { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    [ForeignKey("ToDoId")]
    public virtual ICollection<ToDoUser> ToDoUsers { get; set; } = new List<ToDoUser>();
    public static void ConfigureMap(IMappingExpression<ToDoCreateModel, ToDo> cfg)
    {
        cfg.IgnoreAllMembers().ForMember(x => x.Name, _ => _.MapFrom(x => x.Name))
            .ForMember(x => x.Body, _ => _.MapFrom(x => x.Body));
    }

    public static void ConfigureMap(IMappingExpression<ToDoUpdateModel, ToDo> cfg)
    {
        cfg.IgnoreAllMembers().ForMember(x => x.Name, _ => _.MapFrom(x => x.Name))
            .ForMember(x => x.Body, _ => _.MapFrom(x => x.Body));
    }
}