﻿namespace NeKanban.Logic.Models.DeskModels;

public class DeskRemoveUsersModel
{
    public required List<int> UsersToRemove { get; set; } = new ();
}