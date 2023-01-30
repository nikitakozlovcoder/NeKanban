using AutoMapper;
using NeKanban.Common.Interfaces;
using NeKanban.Common.Models.DeskModels;

namespace NeKanban.Common.Entities;

public class Desk : IHasPk<int>, IMapFrom<DeskCreateModel, Desk>, IMapFrom<DeskUpdateModel, Desk>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? InviteLink { get; set; }
    public virtual ICollection<DeskUser> DeskUsers { get; set; } = new List<DeskUser>();
    public virtual ICollection<Column> Columns { get; set; } = new List<Column>();
    public static void ConfigureMap(IMappingExpression<DeskCreateModel, Desk> cfg)
    {
    }

    public static void ConfigureMap(IMappingExpression<DeskUpdateModel, Desk> cfg)
    {
    }
}