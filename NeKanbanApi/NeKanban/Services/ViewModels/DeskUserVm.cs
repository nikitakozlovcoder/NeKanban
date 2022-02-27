using NeKanban.Constants;

namespace NeKanban.Services.ViewModels;

public class DeskUserVm : BaseIdVm
{
    public ApplicationUserVm? User { get; set; }
    public RoleType Role { get; set; }
    public PreferenceType Preference { get; set; }
    public string RoleName => Role.ToString();
    public string PreferenceName => Preference.ToString();
}