using AutoMapper;
using NeKanban.Common.Constants;
using NeKanban.Common.Interfaces;
using NeKanban.Common.Models.ColumnModels;

namespace NeKanban.Common.Entities;

public class Column : IHasPk<int>, IMapSrcDest<ColumnCreateModel, Column>,  IMapSrcDest<ColumnUpdateModel, Column>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ColumnType Type { get; set; }
    public int Order { get; set; }
    public int DeskId { get; set; }
    public virtual Desk? Desk { get; set; }
    public virtual ICollection<ToDo> ToDos { get; set; } = new List<ToDo>();
    public static IMappingExpression<ColumnCreateModel, Column> ConfigureMap(IMappingExpression<ColumnCreateModel, Column> cfg)
    {
        return cfg;
    }

    public static IMappingExpression<ColumnUpdateModel, Column> ConfigureMap(IMappingExpression<ColumnUpdateModel, Column> cfg)
    {
        return cfg;
    }
}