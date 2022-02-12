namespace NeKanban.Services.ViewModels;

public class ApplicationUserVm
{
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public Token? Token { get; set; }
    public string Email { get; set; } = "";
}