using System.ComponentModel.DataAnnotations;

namespace NeKanban.Common.Models.ToDoModels;

public class ToDoUpdateModel
{
    [Required] 
    [MinLength(3)]
    public required string? Name { get; set; }
    
    [MinLength(10)]
    public required string? Body { get; set; }
}