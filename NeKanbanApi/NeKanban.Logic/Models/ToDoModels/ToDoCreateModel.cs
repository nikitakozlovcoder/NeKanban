﻿using System.ComponentModel.DataAnnotations;

namespace NeKanban.Logic.Models.ToDoModels;

public class ToDoCreateModel
{
    [Required] 
    [MinLength(3)]
    public required string? Name { get; set; }
    
    [MinLength(10)]
    public required string? Body { get; set; }
}
