using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class DeskVm : BaseIdVm, IMapSrcDest<Desk, DeskVm>
{
    public string Name { get; set; } = "";
    public string? InviteLink { get; set; }
    public List<DeskUserLiteVm> DeskUsers { get; set; } = new();

    public static IMappingExpression<Desk, DeskVm> ConfigureMap(IMappingExpression<Desk, DeskVm> cfg)
    {
        return cfg.ForMember(x => x.InviteLink, _ => _.Ignore());
    }
}