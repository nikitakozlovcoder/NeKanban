using AutoMapper;
using NeKanban.Common.DTOs.ApplicationUsers;
using NeKanban.Common.DTOs.Roles;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.DTOs.DesksUsers;

public class DeskUserLiteDto : BaseEntityModel<int>, IMapFrom<DeskUser, DeskUserLiteDto>
{
    public required int UserId { get; set; }
    public required ApplicationUserDto User { get; set; }
    public required bool IsOwner { get; set; }
    public required RoleDto? Role { get; set; }
    public static void ConfigureMap(IMappingExpression<DeskUser, DeskUserLiteDto> cfg)
    {
    }
}