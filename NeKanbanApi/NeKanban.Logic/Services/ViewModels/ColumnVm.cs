using NeKanban.Data.Constants;

namespace NeKanban.Logic.Services.ViewModels;

public class ColumnVm : BaseIdVm
{
    public string? Name { get; set; }
    public ColumnType Type { get; set; }
    public string Typename => Type.ToString();
    public int Order { get; set; }
}