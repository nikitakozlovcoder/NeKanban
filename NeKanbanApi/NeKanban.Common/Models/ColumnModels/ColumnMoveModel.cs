using System.ComponentModel.DataAnnotations;

namespace NeKanban.Common.Models.ColumnModels;

public class ColumnMoveModel
{
    [Range(0, int.MaxValue)]
    public int Position { get; set; }
}