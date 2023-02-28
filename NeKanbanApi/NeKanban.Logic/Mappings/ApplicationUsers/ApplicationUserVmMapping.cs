using Batteries.Injection.Attributes;
using Batteries.Mapper;
using Batteries.Mapper.AppMapper;
using Batteries.Mapper.Interfaces;
using JetBrains.Annotations;
using NeKanban.Common.DTOs.ApplicationUsers;
using NeKanban.Common.ViewModels.ApplicationUsers;

namespace NeKanban.Logic.Mappings.ApplicationUsers;

[UsedImplicitly]
[Injectable<IMappingProfile<ApplicationUserDto, ApplicationUserVm>>]
public class ApplicationUserVmMapping : BaseMappingProfile<ApplicationUserDto, ApplicationUserVm>
{
    public ApplicationUserVmMapping(IAppMapper mapper)
    {
    }

    public override Task<ApplicationUserVm> Map(ApplicationUserDto source, CancellationToken ct)
    {
        return Task.FromResult(new ApplicationUserVm
        {
            Name = source.Name,
            Surname = source.Surname,
            Email = source.Email,
            Id = source.Id
        });

    }
}