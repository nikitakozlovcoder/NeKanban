namespace NeKanban.Controllers.Models.DeskModels;

public class DeskRemoveUsersModel
{
    public List<int> UsersToRemove { get; set; } = new List<int>();
}