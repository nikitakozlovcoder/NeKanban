using AutoMapper;
using Batteries.Mapper.AppMapper.Extensions;
using Batteries.Mapper.Interfaces;
using Batteries.Repository;
using NeKanban.Common.Constants;
using NeKanban.Common.Models.ColumnModels;

namespace NeKanban.Common.Entities;

public class Column : IHasPk<int>, IAutoMapFrom<ColumnCreateModel, Column>,  IAutoMapFrom<ColumnUpdateModel, Column>
{
    public int Id { get; set; }
    public required string Name { get; set; } = "";
    public required ColumnType Type { get; set; }
    public required int Order { get; set; }
    public required int DeskId { get; set; }
    public virtual Desk? Desk { get; set; }
    public virtual ICollection<ToDo> ToDos { get; set; } = new List<ToDo>();
    public static void ConfigureMap(IMappingExpression<ColumnCreateModel, Column> cfg)
    {
        cfg.IgnoreAllMembers()
            .ForMember(x => x.Name, _ => _.MapFrom(x => x.Name));
    }

    public static void ConfigureMap(IMappingExpression<ColumnUpdateModel, Column> cfg)
    {
        cfg.IgnoreAllMembers()
            .ForMember(x => x.Name, _ => _.MapFrom(x => x.Name));
    }
}