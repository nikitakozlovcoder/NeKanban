﻿using System.ComponentModel.DataAnnotations;

namespace NeKanban.Logic.Models.ToDoModels;

public class RemoveUserModel
{
    [Required]
    public int ToDoUserId { get; set; }
}