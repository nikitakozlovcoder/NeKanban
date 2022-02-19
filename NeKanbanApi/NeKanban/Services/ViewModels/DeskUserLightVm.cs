using NeKanban.Constants;

namespace NeKanban.Services.ViewModels;

public class DeskUserLightVm: BaseIdVm
{
    public ApplicationUserVm? User { get; set; }
    public RoleType Role { get; set; }
    public string RoleName => Role.ToString();
}