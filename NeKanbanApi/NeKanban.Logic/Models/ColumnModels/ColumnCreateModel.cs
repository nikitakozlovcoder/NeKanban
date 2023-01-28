using System.ComponentModel.DataAnnotations;

namespace NeKanban.Logic.Models.ColumnModels;

public class ColumnCreateModel
{
    [MinLength(3)] 
    public required string Name { get; set; } = "";
}