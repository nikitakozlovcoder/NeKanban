using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class ApplicationUserVm : BaseIdVm, IMapFrom<ApplicationUser, ApplicationUserVm>
{
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public Token? Token { get; set; }
    public string Email { get; set; } = "";
    
    public static void ConfigureMap(IMappingExpression<ApplicationUser, ApplicationUserVm> cfg)
    {
        cfg.ForMember(x => x.Name, _ => _.MapFrom(x => x.Name ?? string.Empty))
            .ForMember(x => x.Surname, _ => _.MapFrom(x => x.Surname ?? string.Empty));
    }
}