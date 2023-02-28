using Batteries.Injection.Attributes;
using Batteries.Mapper;
using Batteries.Mapper.AppMapper;
using Batteries.Mapper.Interfaces;
using JetBrains.Annotations;
using NeKanban.Common.DTOs.ApplicationUsers;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.DTOs.Roles;
using NeKanban.Common.ViewModels.ApplicationUsers;
using NeKanban.Common.ViewModels.DesksUsers;
using NeKanban.Common.ViewModels.Roles;

namespace NeKanban.Logic.Mappings.DeskUsers;

[UsedImplicitly]
[Injectable<IMappingProfile<DeskUserLiteDto, DeskUserLiteVm>>]
public class DeskUserLiteVmMapping : BaseMappingProfile<DeskUserLiteDto, DeskUserLiteVm>
{
    private readonly IAppMapper _mapper;

    public DeskUserLiteVmMapping(IAppMapper mapper)
    {
        _mapper = mapper;
    }

    public override async Task<DeskUserLiteVm> Map(DeskUserLiteDto source, CancellationToken ct)
    {
        var user = await _mapper.Map<ApplicationUserVm, ApplicationUserDto>(source.User, ct);
        var role = source.Role == null ? null : await _mapper.Map<RoleVm, RoleDto>(source.Role, ct);
        return new DeskUserLiteVm
        {
            UserId = source.UserId,
            User = user,
            IsOwner = source.IsOwner,
            Role = role,
            Id = source.Id
        };
    }
}