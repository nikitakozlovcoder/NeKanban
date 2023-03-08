using AutoMapper;
using Batteries.Mapper.AppMapper.Extensions;
using Batteries.Mapper.Interfaces;
using Batteries.Repository;
using Microsoft.AspNetCore.Identity;
using NeKanban.Common.Models.UserModel;

namespace NeKanban.Common.Entities;

public class ApplicationUser : IdentityUser<int>, IHasPk<int>, IAutoMapFrom<UserRegisterModel, ApplicationUser>
{
    public required string? Name { get; set; }
    public required string? Surname { get; set; }
    public virtual ICollection<UserRefreshToken> RefreshTokens { get; set; } = new List<UserRefreshToken>();
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