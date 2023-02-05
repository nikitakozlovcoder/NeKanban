using AutoMapper;
using NeKanban.Common.DTOs.Permissions;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.DTOs.DesksUsers;

public class DeskUserPermissionsChallengeDto : BaseEntityModel<int>, IMapFrom<DeskUser, DeskUserPermissionsChallengeDto>
{
    public required bool IsOwner { get; set; }
    public required List<RolePermissionDto>? Permissions { get; set; }
    public static void ConfigureMap(IMappingExpression<DeskUser, DeskUserPermissionsChallengeDto> cfg)
    {
        cfg.ForMember(x => x.Permissions, 
            _ => _.MapFrom(x => x.Role != null ? x.Role.Permissions : null ));
    }
}