using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.Roles;
using NeKanban.Common.Entities;
using NeKanban.Common.ViewModels;
using NeKanban.Common.ViewModels.Roles;

namespace NeKanban.Common.DTOs.DesksUsers;

public class DeskUserDto: BaseEntityModel<int>, IAutoMapFrom<DeskUser, DeskUserDto>
{
    public required ApplicationUserWithTokenVm? User { get; set; }
    public required RoleDto? Role { get; set; }
    public required bool IsOwner { get; set; }
    public required PreferenceType Preference { get; set; }
    public string PreferenceName => Preference.ToString();

    public static void ConfigureMap(IMappingExpression<DeskUser, DeskUserDto> cfg)
    {
    }
}