using System.ComponentModel.DataAnnotations;

namespace NeKanban.Common.Models.DeskModels;

public class DeskCreateModel
{
    [MinLength(6)]
    public required string Name { get; set; }
}