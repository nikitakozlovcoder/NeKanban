using System.ComponentModel.DataAnnotations;

namespace NeKanban.Common.Models.ToDoModels;

public class RemoveUserModel
{
    [Required]
    public int ToDoUserId { get; set; }
}