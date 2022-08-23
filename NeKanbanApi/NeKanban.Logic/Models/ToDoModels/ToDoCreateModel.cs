using System.ComponentModel.DataAnnotations;

namespace NeKanban.Logic.Models.ToDoModels;

public class ToDoCreateModel
{
    [Required] 
    [MinLength(3)]
    public string? Name { get; set; }
    [MinLength(10)]
    public string? Body { get; set; }
}
