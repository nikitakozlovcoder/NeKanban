using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NeKanban.Common.Extensions;
using NeKanban.Common.Interfaces;
using NeKanban.Common.Models.UserModel;

namespace NeKanban.Common.Entities;

public class ApplicationUser : IdentityUser<int>, IHasPk<int>, IMapFrom<UserRegisterModel, ApplicationUser>
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public ApplicationUser()
    {
        base.UserName = Guid.NewGuid().ToString();
    }
    
    public static void ConfigureMap(IMappingExpression<UserRegisterModel, ApplicationUser> cfg)
    {
        cfg.IgnoreAllMembers().ForMember(x => x.Name, _ => _.MapFrom(x => x.Name))
            .ForMember(x => x.Surname, _ => _.MapFrom(x => x.Surname))
            .ForMember(x => x.Email, _ => _.MapFrom(x => x.Email));
    }
}