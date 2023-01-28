using System.ComponentModel.DataAnnotations;

namespace NeKanban.Logic.Models.DeskModels;

public class DeskCreateModel
{
    [MinLength(6)]
    public required string Name { get; set; } = "";
}