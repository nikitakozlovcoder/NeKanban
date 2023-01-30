using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NeKanban.Common.Interfaces;
using NeKanban.Common.Models.UserModel;

namespace NeKanban.Common.Entities;

public class ApplicationUser : IdentityUser<int>, IHasPk<int>, IMapFrom<UserRegisterModel, ApplicationUser>
{
    public ApplicationUser()
    {
        base.UserName = Guid.NewGuid().ToString();
    }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public static void ConfigureMap(IMappingExpression<UserRegisterModel, ApplicationUser> cfg)
    {
    }
}