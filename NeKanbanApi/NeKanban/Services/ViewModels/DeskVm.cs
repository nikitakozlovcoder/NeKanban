namespace NeKanban.Services.ViewModels;

public class DeskVm
{
    public string Name { get; set; } = "";
    public string? InviteLink { get; set; }
    public List<DeskUserVm> DeskUsers {get; set; }
}