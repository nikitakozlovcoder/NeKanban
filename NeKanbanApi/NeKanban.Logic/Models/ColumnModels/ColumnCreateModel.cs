using System.ComponentModel.DataAnnotations;

namespace NeKanban.Logic.Models.ColumnModels;

public class ColumnCreateModel
{
    [MinLength(3)] 
    public string Name { get; set; } = "";
}