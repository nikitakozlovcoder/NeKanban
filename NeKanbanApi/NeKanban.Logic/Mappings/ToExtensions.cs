﻿using NeKanban.Data.Entities;
using NeKanban.Logic.Models.DeskModels;
using NeKanban.Logic.SecurityProfile.Helpers;
using NeKanban.Logic.Services.ViewModels;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.Mappings;

public static class ToExtensions
{
    #region User
    public static ApplicationUserVm ToApplicationUserVm(this ApplicationUser applicationUser, Token? token = null)
    {
        return new ApplicationUserVm
        {
            Id = applicationUser.Id,
            Name = applicationUser.Name ?? "",
            Surname = applicationUser.Surname ?? "",
            Token = token,
            Email = applicationUser.Email
        };
    }
    #endregion
    #region Desk
    public static Desk ToDesk(this DeskCreateModel deskCreateModel)
    {
        return new Desk
        {
            Name = deskCreateModel.Name,
        };
    }
    
    public static DeskVm ToDeskVm(this Desk desk, int? userId)
    {
        var role = userId.HasValue ? desk.DeskUsers.FirstOrDefault(x => x.UserId == userId)?.Role : null;
        var canViewInviteLink = role.HasValue && PermissionChecker.CheckPermission(role.Value, PermissionType.ViewInviteLink);
        return new DeskVm
        {
            Id = desk.Id,
            Name = desk.Name,
            InviteLink =  canViewInviteLink ? desk.InviteLink : null,
            DeskUsers = desk.DeskUsers.Select(deskUser => deskUser.ToDeskUserLiteVm()).ToList()
        };
    }
    
    public static DeskUserVm ToDeskUserVm(this DeskUser deskUser)
    {
        return new DeskUserVm
        {
            Id = deskUser.Id,
            User = deskUser.User?.ToApplicationUserVm(),
            Role = deskUser.Role,
            Preference = deskUser.Preference
        };
    }
    public static DeskUserLiteVm ToDeskUserLiteVm(this DeskUser deskUser)
    {
        return new DeskUserLiteVm
        {
            Id = deskUser.Id,
            User = deskUser.User?.ToApplicationUserVm(),
            Role = deskUser.Role,
        };
    }
    
    public static DeskLiteVm ToDeskLiteVm(this Desk desk)
    {
        return new DeskLiteVm
        {
            Id = desk.Id,
            Name = desk.Name,
            DeskUser = desk.DeskUsers.FirstOrDefault()?.ToDeskUserVm()
        };
    }
    #endregion
    #region Column

    public static ColumnVm ToColumnVm(this Column column)
    {
        return new ColumnVm
        {
            Id = column.Id,
            Order = column.Order,
            Name = column.Name,
            Type = column.Type
        };
    }

    #endregion
    #region ToDo

    public static ToDoVm ToToDoVm(this ToDo toDo)
    {
        return new ToDoVm
        {
            Id = toDo.Id,
            Order = toDo.Order,
            Column = toDo.Column?.ToColumnVm(),
            Body = toDo.Body,
            Name = toDo.Name,
            ToDoUsers  = toDo.ToDoUsers.Select(x=> x.ToToDoUserVm()).ToList()
        };
    }
    
    public static ToDoUserVm ToToDoUserVm(this ToDoUser toDoUser)
    {
        return new ToDoUserVm
        {
            Id = toDoUser.Id,
            ToDoUserType = toDoUser.ToDoUserType,
            DeskUser = toDoUser.DeskUser?.ToDeskUserLiteVm()
        };
    }

    #endregion
}