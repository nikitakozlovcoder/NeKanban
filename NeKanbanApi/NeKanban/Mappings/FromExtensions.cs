using NeKanban.Controllers.Models;
using NeKanban.Data.Entities;

namespace NeKanban.Mappings;

public static class FromExtensions
{
    public static void FromRegistrationModel(this ApplicationUser applicationUser,  UserRegisterModel userRegister)
    {
        applicationUser.Email = userRegister.Email;
        applicationUser.Name = userRegister.Name;
        applicationUser.Surname = userRegister.Surname;
    }
}