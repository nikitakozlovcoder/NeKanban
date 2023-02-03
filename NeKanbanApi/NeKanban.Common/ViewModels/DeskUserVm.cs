using AutoMapper;
using NeKanban.Common.Constants;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;
using NeKanban.Security.Constants;

namespace NeKanban.Common.ViewModels;

public class DeskUserVm : BaseIdVm, IMapFrom<DeskUser, DeskUserVm>
{
    public required ApplicationUserVm? User { get; set; }
    public required RoleType Role { get; set; }
    public required PreferenceType Preference { get; set; }
    public string RoleName => Role.ToString();
    public string PreferenceName => Preference.ToString();
    public static void ConfigureMap(IMappingExpression<DeskUser, DeskUserVm> cfg)
    {
    }
}