namespace NeKanban.Controllers.Models.UserModel;

public class UserRegisterModel : UserLoginModel
{
   public string? Name { get; set; }
   public string? Surname { get; set; }
}