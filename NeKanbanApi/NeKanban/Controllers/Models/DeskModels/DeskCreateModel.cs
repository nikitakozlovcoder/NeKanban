using System.ComponentModel.DataAnnotations;

namespace NeKanban.Controllers.Models.DeskModels;

public class DeskCreateModel
{
    [MinLength(6)]
    public string Name { get; set; } = "";
}