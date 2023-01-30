﻿using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;
using NeKanban.Security.Constants;

namespace NeKanban.Common.ViewModels;

public class DeskUserLiteVm: BaseIdVm, IMapFrom<DeskUser, DeskUserLiteVm>
{
    public ApplicationUserVm? User { get; set; }
    public RoleType Role { get; set; }
    public string RoleName => Role.ToString();
    public static void ConfigureMap(IMappingExpression<DeskUser, DeskUserLiteVm> cfg)
    {
    }
}