using NeKanban.Constants;

namespace NeKanban.Services.ViewModels;

public class DeskUserVm
{
    public ApplicationUserVm? User { get; set; }
    public RoleType Role { get; set; }
    public string? RoleName { get; set; }
}