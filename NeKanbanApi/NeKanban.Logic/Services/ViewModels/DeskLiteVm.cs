namespace NeKanban.Logic.Services.ViewModels;

public class DeskLiteVm : BaseIdVm
{
    public required string? Name { get; set; }
    public required DeskUserVm? DeskUser { get; set; }
    
}