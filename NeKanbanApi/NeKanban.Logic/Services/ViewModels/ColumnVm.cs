using NeKanban.Data.Constants;

namespace NeKanban.Logic.Services.ViewModels;

public class ColumnVm : BaseIdVm
{
    public required string? Name { get; set; }
    public required ColumnType Type { get; set; }
    public string Typename => Type.ToString();
    public required int Order { get; set; }
}