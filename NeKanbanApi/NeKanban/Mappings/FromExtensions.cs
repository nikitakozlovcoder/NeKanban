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

    public static void FromUpdateModel(this Desk desk, DeskUpdateModel deskUpdateModel)
    {
        desk.Name = deskUpdateModel.Name;
    }
    
}