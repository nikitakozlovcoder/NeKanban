using System.ComponentModel.DataAnnotations;

namespace NeKanban.Common.Models.RoleModels;

public class CreateRoleModel
{
    [Required]
    [MinLength(5)]
    public required string Name { get; set; }
}