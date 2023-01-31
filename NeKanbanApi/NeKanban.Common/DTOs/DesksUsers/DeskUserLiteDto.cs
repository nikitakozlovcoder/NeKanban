using AutoMapper;
using NeKanban.Common.DTOs.ApplicationUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;
using NeKanban.Security.Constants;

namespace NeKanban.Common.DTOs.DesksUsers;

public class DeskUserLiteDto : BaseEntityDto<int>, IMapFrom<DeskUser, DeskUserLiteDto>
{
    public int UserId { get; set; }
    public ApplicationUserDto User { get; set; } = null!;
    public RoleType Role { get; set; }
    public static void ConfigureMap(IMappingExpression<DeskUser, DeskUserLiteDto> cfg)
    {
    }
}