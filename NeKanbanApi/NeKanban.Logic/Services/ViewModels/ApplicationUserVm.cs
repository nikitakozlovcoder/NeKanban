namespace NeKanban.Logic.Services.ViewModels;

public class ApplicationUserVm : BaseIdVm
{
    public required string Name { get; set; } = "";
    public required string Surname { get; set; } = "";
    public required Token? Token { get; set; }
    public required string Email { get; set; } = "";
}