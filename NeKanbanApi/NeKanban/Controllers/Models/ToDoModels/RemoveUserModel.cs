using System.ComponentModel.DataAnnotations;

namespace NeKanban.Controllers.Models.ToDoModels;

public class RemoveUserModel
{
    [Required]
    public int ToDoUserId { get; set; }
}