using System.ComponentModel.DataAnnotations;

namespace NeKanban.Controllers.Models.ToDoModels;

public class AssignUserModel
{
    [Required]
    public int DescUserId { get; set; }
}