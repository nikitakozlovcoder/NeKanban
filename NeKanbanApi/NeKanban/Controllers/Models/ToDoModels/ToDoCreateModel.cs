using System.ComponentModel.DataAnnotations;

namespace NeKanban.Controllers.Models.ToDoModels;

public class ToDoCreateModel
{
    [Required] 
    public string? Name { get; set; }
    public string? Body { get; set; }
}