using AutoMapper;
using NeKanban.Common.DTOs.Desks;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class DeskVm : BaseIdVm, IMapFrom<DeskDto, DeskVm>
{
    public string Name { get; set; } = "";
    public string? InviteLink { get; set; }
    public List<DeskUserLiteVm> DeskUsers { get; set; } = new();
    public static void ConfigureMap(IMappingExpression<DeskDto, DeskVm> cfg)
    {
    }
}