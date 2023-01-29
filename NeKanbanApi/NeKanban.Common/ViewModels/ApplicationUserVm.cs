using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class ApplicationUserVm : BaseIdVm, IMapSrcDest<ApplicationUser, ApplicationUserVm>
{
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public Token? Token { get; set; }
    public string Email { get; set; } = "";
    
    public static IMappingExpression<ApplicationUser, ApplicationUserVm> ConfigureMap(IMappingExpression<ApplicationUser, ApplicationUserVm> cfg)
    {
        return cfg
            .ForMember(x => x.Name, _ => _.MapFrom(x => x.Name ?? string.Empty))
            .ForMember(x => x.Surname, _ => _.MapFrom(x => x.Surname ?? string.Empty));
    }
}