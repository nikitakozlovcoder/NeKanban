namespace NeKanban.Logic.Services.ViewModels;

public class DeskVm : BaseIdVm
{
    public string Name { get; set; } = "";
    public string? InviteLink { get; set; }
    public List<DeskUserLightVm> DeskUsers { get; set; } = new List<DeskUserLightVm>();
}