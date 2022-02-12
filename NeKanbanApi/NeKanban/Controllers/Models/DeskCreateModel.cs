using System.ComponentModel.DataAnnotations;

namespace NeKanban.Controllers.Models;

public class DeskCreateModel
{
    [MinLength(8)]
    public string Name { get; set; } = "";
}