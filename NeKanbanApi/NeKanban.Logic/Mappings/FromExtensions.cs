using NeKanban.Data.Entities;
using NeKanban.Logic.Models.ColumnModels;
using NeKanban.Logic.Models.DeskModels;
using NeKanban.Logic.Models.ToDoModels;
using NeKanban.Logic.Models.UserModel;

namespace NeKanban.Logic.Mappings;

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
    public static void FromUpdateModel(this ToDo toDo, ToDoUpdateModel model)
    {
        toDo.Name = model.Name ?? "";
        toDo.Body = model.Body;
    }
}