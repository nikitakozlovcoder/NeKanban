using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.ApplicationUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.ViewModels.Roles;

namespace NeKanban.Common.DTOs.DesksUsers;

public class DeskUserDto: BaseEntityModel<int>, IAutoMapFrom<DeskUser, DeskUserDto>
{
    public required ApplicationUserDto? User { get; set; }
    public required RoleDto? Role { get; set; }
    public required bool IsOwner { get; set; }
    public DeskUserDeletionReason? DeletionReason { get; set; }
    public required PreferenceType Preference { get; set; }
    public string PreferenceName => Preference.ToString();

    public static void ConfigureMap(IMappingExpression<DeskUser, DeskUserDto> cfg)
    {
    }
}