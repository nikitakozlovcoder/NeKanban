using AutoMapper;
using NeKanban.Common.Interfaces;
using NeKanban.Common.Models.DeskModels;

namespace NeKanban.Common.Entities;

public class Desk : IHasPk<int>, IMapSrcDest<DeskCreateModel, Desk>, IMapSrcDest<DeskUpdateModel, Desk>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? InviteLink { get; set; }
    public virtual ICollection<DeskUser> DeskUsers { get; set; } = new List<DeskUser>();
    public virtual ICollection<Column> Columns { get; set; } = new List<Column>();
    public static IMappingExpression<DeskCreateModel, Desk> ConfigureMap(IMappingExpression<DeskCreateModel, Desk> cfg)
    {
        return cfg;
    }

    public static IMappingExpression<DeskUpdateModel, Desk> ConfigureMap(IMappingExpression<DeskUpdateModel, Desk> cfg)
    {
        return cfg;
    }
}