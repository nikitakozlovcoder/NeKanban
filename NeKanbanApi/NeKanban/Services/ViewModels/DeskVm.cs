namespace NeKanban.Services.ViewModels;

public class DeskVm : BaseIdVm
{
    public string Name { get; set; } = "";
    public string? InviteLink { get; set; }
    public List<DeskUserVm> DeskUsers { get; set; } = new List<DeskUserVm>();
}