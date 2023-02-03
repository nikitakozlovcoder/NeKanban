using AutoMapper;
using NeKanban.Common.DTOs.ApplicationUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class ApplicationUserVm : BaseIdVm, IMapFrom<ApplicationUser, ApplicationUserVm>, IMapFrom<ApplicationUserDto, ApplicationUserVm>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required Token? Token { get; set; }
    public required string Email { get; set; }
    
    public static void ConfigureMap(IMappingExpression<ApplicationUser, ApplicationUserVm> cfg)
    {
        cfg.ForMember(x => x.Name, _ => _.MapFrom(x => x.Name ?? string.Empty))
            .ForMember(x => x.Surname, _ => _.MapFrom(x => x.Surname ?? string.Empty))
            .ForMember(x => x.Token, _ => _.Ignore());
    }

    public static void ConfigureMap(IMappingExpression<ApplicationUserDto, ApplicationUserVm> cfg)
    {
        cfg.ForMember(x => x.Token, _ => _.Ignore());
    }
}