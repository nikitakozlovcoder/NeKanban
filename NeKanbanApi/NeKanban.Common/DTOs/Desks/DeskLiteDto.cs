using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;

namespace NeKanban.Common.DTOs.Desks;

public class DeskLiteDto : BaseEntityModel<int>, IAutoMapFrom<Desk, DeskLiteDto>
{
    public required string? Name { get; set; }
    public required DeskUserDto? DeskUser { get; set; }
    public static void ConfigureMap(IMappingExpression<Desk, DeskLiteDto> cfg)
    {
        cfg.ForMember( x => x.DeskUser, _ => _.MapFrom(x => x.DeskUsers.SingleOrDefault()));
    }
}