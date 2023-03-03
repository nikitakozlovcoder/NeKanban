namespace NeKanban.Common.Models.UserModel;

public class UserRegisterModel : UserLoginModel
{
   public required string? Name { get; set; }
   public required string? Surname { get; set; }
   public required bool PersonalDataAgreement { get; set; }
}