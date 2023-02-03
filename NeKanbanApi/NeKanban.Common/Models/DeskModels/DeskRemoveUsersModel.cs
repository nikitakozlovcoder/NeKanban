namespace NeKanban.Common.Models.DeskModels;

public class DeskRemoveUsersModel
{
    public required List<int> UsersToRemove { get; set; } = new ();
}