using AutoMapper;
using NeKanban.Common.Extensions;
using NeKanban.Common.Interfaces;
using NeKanban.Common.Models.RoleModels;

namespace NeKanban.Common.Entities;

public class Role : IHasPk<int>, IAutoMapFrom<CreateRoleModel, Role>, IAutoMapFrom<UpdateRoleModel, Role>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int DeskId { get; set; }
    public required bool IsDefault { get; set; }
    public virtual Desk? Desk { get; set; }
    public virtual ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
    public static void ConfigureMap(IMappingExpression<CreateRoleModel, Role> cfg)
    {
        cfg.IgnoreAllMembers().ForMember(x => x.Name, _ => _.MapFrom(x => x.Name));
    }

    public static void ConfigureMap(IMappingExpression<UpdateRoleModel, Role> cfg)
    {
        cfg.IgnoreAllMembers()
            .ForMember(x => x.Name, _ => _.MapFrom(x => x.Name));
    }
}