using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;

namespace NeKanban.Common.DTOs.Desks;

public class DeskDto : BaseEntityModel<int>,  IAutoMapFrom<Desk, DeskDto>
{
    public required string Name { get; set; }
    public required string? InviteLink { get; set; }
    public required List<DeskUserLiteDto> DeskUsers { get; set; }
    public static void ConfigureMap(IMappingExpression<Desk, DeskDto> cfg)
    {
    }
}