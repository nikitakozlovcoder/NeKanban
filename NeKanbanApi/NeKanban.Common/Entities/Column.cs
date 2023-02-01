using AutoMapper;
using NeKanban.Common.Constants;
using NeKanban.Common.Extensions;
using NeKanban.Common.Interfaces;
using NeKanban.Common.Models.ColumnModels;

namespace NeKanban.Common.Entities;

public class Column : IHasPk<int>, IMapFrom<ColumnCreateModel, Column>,  IMapFrom<ColumnUpdateModel, Column>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ColumnType Type { get; set; }
    public int Order { get; set; }
    public int DeskId { get; set; }
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