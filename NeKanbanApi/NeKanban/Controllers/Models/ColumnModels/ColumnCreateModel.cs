using System.ComponentModel.DataAnnotations;

namespace NeKanban.Controllers.Models.ColumnModels;

public class ColumnCreateModel
{
    [MinLength(3)] 
    public string Name { get; set; } = "";
}