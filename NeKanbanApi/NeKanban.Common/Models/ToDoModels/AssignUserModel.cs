using System.ComponentModel.DataAnnotations;

namespace NeKanban.Common.Models.ToDoModels;

public class AssignUserModel
{
    [Required]
    public required int DeskUserId { get; set; }
}