using System.ComponentModel.DataAnnotations;

namespace NeKanban.Common.Models.ToDoModels;

public class AssignUserModel
{
    [Required]
    public int DescUserId { get; set; }
}