using NeKanban.Security.Constants;

namespace NeKanban.Logic.Services.ViewModels;

public class DeskUserLiteVm: BaseIdVm
{
    public ApplicationUserVm? User { get; set; }
    public RoleType Role { get; set; }
    public string RoleName => Role.ToString();
}