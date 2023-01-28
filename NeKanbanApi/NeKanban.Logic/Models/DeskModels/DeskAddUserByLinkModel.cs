using System.ComponentModel.DataAnnotations;

namespace NeKanban.Logic.Models.DeskModels;

public class DeskAddUserByLinkModel
{
    [Required(AllowEmptyStrings = false)]
    public required string Uid { get; set; } = "";
}