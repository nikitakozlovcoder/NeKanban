using JetBrains.Annotations;
using NeKanban.Common.Attributes;
using NeKanban.Common.DTOs.ApplicationUsers;
using NeKanban.Common.Interfaces;
using NeKanban.Common.ViewModels;

namespace NeKanban.Logic.Mappings;

[UsedImplicitly]
[Injectable<IMappingProfile<ApplicationUserDto, ApplicationUserWithAvatarVm>>]
public class ApplicationUserMappings : BaseMappingProfile<ApplicationUserDto, ApplicationUserWithAvatarVm>
{
    public override Task<ApplicationUserWithAvatarVm> Map(ApplicationUserDto source, CancellationToken ct)
    {
        return Task.FromResult(new ApplicationUserWithAvatarVm
        {
            Id = source.Id,
            Name = source.Name,
            Surname = source.Surname,
            Email = source.Surname,
            AvatarUrl = "",
        });
    }
}