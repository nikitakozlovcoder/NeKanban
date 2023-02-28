using System.ComponentModel.DataAnnotations;

namespace NeKanban.Common.Models.ToDoModels;

public class ToDoCreateModel
{
    [Required] 
    [MinLength(3)]
    public required string? Name { get; set; }
}
