using NeKanban.Data.Constants;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.Services.ViewModels;

public class DeskUserVm : BaseIdVm
{
    public required ApplicationUserVm? User { get; set; }
    public required RoleType Role { get; set; }
    public required PreferenceType Preference { get; set; }
    public string RoleName => Role.ToString();
    public string PreferenceName => Preference.ToString();
}