using System.ComponentModel.DataAnnotations;

namespace NeKanban.Common.Models.ToDoModels;

public class RemoveUserModel
{
    [Required]
    public required int ToDoUserId { get; set; }
}