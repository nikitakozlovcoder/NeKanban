using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class DeskLiteVm : BaseIdVm, IMapSrcDest<Desk, DeskLiteVm>
{
    public string? Name { get; set; }
    public DeskUserVm? DeskUser { get; set; }
    public static IMappingExpression<Desk, DeskLiteVm> ConfigureMap(IMappingExpression<Desk, DeskLiteVm> cfg)
    {
        return cfg.ForMember( x => x.DeskUser, _ => _.MapFrom(x => x.DeskUsers.SingleOrDefault()));
    }
}