﻿namespace NeKanban.Common.Models.UserModel;

public class UserLoginModel
{
    public required string Email { get; set; } = "";
    public required string Password { get; set; } = "";
}