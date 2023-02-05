namespace NeKanban.Common.Models.RoleModels;

public class UpdateRoleModel : CreateRoleModel
{
    public required bool IsDefault { get; set; }
}