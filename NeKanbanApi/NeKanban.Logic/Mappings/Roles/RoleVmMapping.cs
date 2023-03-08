using Batteries.Injection.Attributes;
using Batteries.Mapper;
using Batteries.Mapper.AppMapper;
using Batteries.Mapper.Interfaces;
using JetBrains.Annotations;
using NeKanban.Common.DTOs.Roles;
using NeKanban.Common.ViewModels.Roles;

namespace NeKanban.Logic.Mappings.Roles;

[UsedImplicitly]
[Injectable<IMappingProfile<RoleDto, RoleVm>>]
public class RoleVmMapping : BaseMappingProfile<RoleDto, RoleVm>
{
    public RoleVmMapping(IAppMapper mapper)
    {
    }

    public override Task<RoleVm> Map(RoleDto source, CancellationToken ct)
    {
        return Task.FromResult(new RoleVm
        {
            Name = source.Name,
            IsDefault = source.IsDefault,
            DeskId = source.DeskId,
            Id = source.Id
        });
    }
}