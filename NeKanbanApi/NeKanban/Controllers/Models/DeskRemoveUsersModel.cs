namespace NeKanban.Controllers.Models;

public class DeskRemoveUsersModel
{
    public List<int> UsersToRemove { get; set; } = new List<int>();
}