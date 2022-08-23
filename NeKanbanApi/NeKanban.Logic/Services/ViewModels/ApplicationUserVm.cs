namespace NeKanban.Logic.Services.ViewModels;

public class ApplicationUserVm : BaseIdVm
{
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public Token? Token { get; set; }
    public string Email { get; set; } = "";
}