using System.ComponentModel.DataAnnotations;

namespace NeKanban.Logic.Models.ToDoModels;

public class AssignUserModel
{
    [Required]
    public required int DescUserId { get; set; }
}