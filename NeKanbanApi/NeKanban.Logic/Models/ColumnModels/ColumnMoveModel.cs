using System.ComponentModel.DataAnnotations;

namespace NeKanban.Logic.Models.ColumnModels;

public class ColumnMoveModel
{
    [Range(0, int.MaxValue)]
    public required int Position { get; set; }
}