using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class DeskVm : BaseIdVm, IMapFrom<Desk, DeskVm>
{
    public string Name { get; set; } = "";
    public string? InviteLink { get; set; }
    public List<DeskUserLiteVm> DeskUsers { get; set; } = new();

    public static void ConfigureMap(IMappingExpression<Desk, DeskVm> cfg)
    {
        cfg.ForMember(x => x.InviteLink, _ => _.Ignore());
    }
}