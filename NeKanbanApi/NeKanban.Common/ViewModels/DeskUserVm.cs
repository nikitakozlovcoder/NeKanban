using AutoMapper;
using NeKanban.Common.Constants;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class DeskUserVm : BaseIdVm, IMapFrom<DeskUser, DeskUserVm>
{
    public required ApplicationUserVm? User { get; set; }
    public required RoleVm? Role { get; set; }
    public required bool IsOwner { get; set; }
    public required PreferenceType Preference { get; set; }
    public string PreferenceName => Preference.ToString();
    public static void ConfigureMap(IMappingExpression<DeskUser, DeskUserVm> cfg)
    {
    }
}