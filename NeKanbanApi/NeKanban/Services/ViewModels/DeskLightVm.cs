using NeKanban.Constants;

namespace NeKanban.Services.ViewModels;

public class DeskLightVm : BaseIdVm
{
    public string? Name { get; set; }
    public DeskUserVm? DeskUser { get; set; }
    
}