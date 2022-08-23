using System.ComponentModel.DataAnnotations;

namespace NeKanban.Logic.Models.ToDoModels;

public class AssignUserModel
{
    [Required]
    public int DescUserId { get; set; }
}