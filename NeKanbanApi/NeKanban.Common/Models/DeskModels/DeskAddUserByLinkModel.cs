using System.ComponentModel.DataAnnotations;

namespace NeKanban.Common.Models.DeskModels;

public class DeskAddUserByLinkModel
{
    [Required(AllowEmptyStrings = false)]
    public required string Uid { get; set; }
}