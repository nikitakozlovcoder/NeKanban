using NeKanban.Controllers;
using NeKanban.Controllers.Models;
using NeKanban.Controllers.Models.ColumnModels;
using NeKanban.Controllers.Models.DeskModels;
using NeKanban.Controllers.Models.ToDoModels;
using NeKanban.Controllers.Models.UserModel;
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
    
    public static void FromCreateModel(this Column column, ColumnCreateModel model)
    {
        column.Name = model.Name;
    }
    public static void FromUpdateModel(this Column column, ColumnUpdateModel model)
    {
        column.Name = model.Name;
    }
    
    public static void FromCreateModel(this ToDo toDo, ToDoCreateModel model)
    {
        toDo.Name = model.Name ?? "";
        toDo.Body = model.Body;
    }
    
    
}