using NeKanban.Constants;
using NeKanban.Controllers.Models;
using NeKanban.Data.Entities;
using NeKanban.Services.ViewModels;

namespace NeKanban.Mappings;

public static class ToExtensions
{
    public static ApplicationUserVm ToApplicationUserVm(this ApplicationUser applicationUser, Token? token = null)
    {
        return new ApplicationUserVm
        {
            Id = applicationUser.Id,
            Name = applicationUser.Name ?? "",
            Surname = applicationUser.Surname ?? "",
            Token = token,
            Email = applicationUser.Email
        };
    }
    
    public static Desk ToDesk(this DeskCreateModel deskCreateModel)
    {
        return new Desk
        {
            Name = deskCreateModel.Name,
        };
    }
    
    public static DeskVm ToDeskVm(this Desk desk)
    {
       return new DeskVm
        {
            Id = desk.Id,
            Name = desk.Name,
            InviteLink = desk.InviteLink,
            DeskUsers = desk.DeskUsers.Select(deskUser => deskUser.ToDeskUserVm()).ToList()
        };
    }
    
    public static DeskUserVm ToDeskUserVm(this DeskUser deskUser)
    {
        return new DeskUserVm
        {
            Id = deskUser.Id,
            User = deskUser.User?.ToApplicationUserVm(),
            Role = deskUser.Role,
            Preference = deskUser.Preference
        };
    }
    
    public static DeskLightVm ToDeskLightVm(this Desk desk)
    {
        return new DeskLightVm
        {
            Id = desk.Id,
            Name = desk.Name,
            DeskUser = desk.DeskUsers.FirstOrDefault()?.ToDeskUserVm()
        };
    }
    
    
}