using System.ComponentModel.DataAnnotations;

namespace NeKanban.Logic.Models.ToDoModels;

public class RemoveUserModel
{
    [Required]
    public required int ToDoUserId { get; set; }
}