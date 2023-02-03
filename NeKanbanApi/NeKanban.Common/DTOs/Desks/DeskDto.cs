using AutoMapper;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.DTOs.Desks;

public class DeskDto : BaseEntityDto<int>,  IMapFrom<Desk, DeskDto>
{
    public required string Name { get; set; }
    public required string? InviteLink { get; set; }
    public required List<DeskUserLiteDto> DeskUsers { get; set; } = new();
    public static void ConfigureMap(IMappingExpression<Desk, DeskDto> cfg)
    {
    }
}