using NeKanban.Security.Constants;

namespace NeKanban.Logic.Services.ViewModels;

public class DeskUserLiteVm: BaseIdVm
{
    public required ApplicationUserVm? User { get; set; }
    public required RoleType Role { get; set; }
    public string RoleName => Role.ToString();
}