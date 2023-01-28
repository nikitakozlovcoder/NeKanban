namespace NeKanban.Logic.Services.ViewModels;

public class DeskVm : BaseIdVm
{
    public string Name { get; set; } = "";
    public string? InviteLink { get; set; }
    public List<DeskUserLiteVm> DeskUsers { get; set; } = new();
}