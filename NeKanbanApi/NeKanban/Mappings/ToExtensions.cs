﻿using NeKanban.Constants;
using NeKanban.Constants.Security;
using NeKanban.Controllers.Models;
using NeKanban.Controllers.Models.DeskModels;
using NeKanban.Data.Entities;
using NeKanban.Helpers;
using NeKanban.Services.ViewModels;

namespace NeKanban.Mappings;

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
            DeskUsers = desk.DeskUsers.Select(deskUser => deskUser.ToDeskUserLightVm()).ToList()
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
    public static DeskUserLightVm ToDeskUserLightVm(this DeskUser deskUser)
    {
        return new DeskUserLightVm
        {
            Id = deskUser.Id,
            User = deskUser.User?.ToApplicationUserVm(),
            Role = deskUser.Role,
        };
    }
    
    public static DeskLightVm ToDeskLightVm(this Desk desk)
    {
        return new DeskLightVm
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
    
}