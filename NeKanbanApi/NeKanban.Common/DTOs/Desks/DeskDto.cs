using AutoMapper;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.DTOs.Desks;

public class DeskDto : BaseEntityDto<int>,  IMapFrom<Desk, DeskDto>
{
    public string Name { get; set; } = "";
    public string? InviteLink { get; set; }
    public List<DeskUserLiteDto> DeskUsers { get; set; } = new();
    public static void ConfigureMap(IMappingExpression<Desk, DeskDto> cfg)
    {
    }
}