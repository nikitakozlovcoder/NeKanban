namespace NeKanban.Logic.Services.ViewModels;

public class DeskVm : BaseIdVm
{
    public required string Name { get; set; } = "";
    public required string? InviteLink { get; set; }
    public List<DeskUserLiteVm> DeskUsers { get; set; } = new List<DeskUserLiteVm>();
}