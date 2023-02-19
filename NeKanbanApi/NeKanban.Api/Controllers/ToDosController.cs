﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Common.DTOs.ToDos;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.ToDoModels;
using NeKanban.Controllers.Auth;
using NeKanban.Logic.Services.DesksUsers;
using NeKanban.Logic.Services.ToDos;
using NeKanban.Logic.Services.ToDos.ToDoUsers;
using NeKanban.Security.Constants;

namespace NeKanban.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class ToDosController : BaseAuthController
{
    private readonly IToDoService _toDoService;
    private readonly IToDoUserService _toDoUserService;
    private readonly IDeskUserService _deskUserService;
    public ToDosController(UserManager<ApplicationUser> userManager,
        IToDoService toDoService,
        IToDoUserService toDoUserService,
        IServiceProvider serviceProvider,
        IDeskUserService deskUserService) : base(userManager, serviceProvider)
    {
        _toDoService = toDoService;
        _toDoUserService = toDoUserService;
        _deskUserService = deskUserService;
    }
    
    [HttpGet("{deskId:int}")]
    public async Task<List<ToDoDto>> GetTodos(int deskId, CancellationToken ct)
    {
        await EnsureAbleTo<Desk>(deskId, ct);
        return await _toDoService.GetToDos(deskId, ct);
    }
    
    [HttpPost("{deskId:int}")]
    public async Task<List<ToDoDto>> CreateToDo(int deskId, [FromBody]ToDoCreateModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Desk>(PermissionType.CreateTasks, deskId, ct);
        var user = await GetApplicationUser();
        return await _toDoService.CreateToDo(deskId, user, model, ct);
    }
    
    [HttpPut("{toDoId:int}")]
    public async Task<ToDoDto> UpdateToDo(int toDoId, [FromBody]ToDoUpdateModel model, CancellationToken ct)
    {
        await EnsureAbleTo<ToDo>(PermissionType.UpdateTask, toDoId, ct);
        return await _toDoService.UpdateToDo(toDoId, model, ct);
    }
    
    [HttpPut("{toDoId:int}")]
    public async Task<ToDoDto> AssignUser(int toDoId, [FromBody]AssignUserModel model, CancellationToken ct)
    {
        var userId= await _deskUserService.GetDeskUserUserId(model.DeskUserId, ct);
        var currentUser = await GetApplicationUser();
        await EnsureAbleTo<ToDo>(userId == currentUser.Id ? PermissionType.AssignTasksThemself : PermissionType.ManageAssigners, toDoId, ct);
        return await _toDoUserService.AssignUser(toDoId, model.DeskUserId, ct);
    }
    
    [HttpDelete]
    public async Task<ToDoDto> RemoveUser([FromBody]RemoveUserModel model, CancellationToken ct)
    {
        var userId= await _toDoUserService.GetToDoUserUserId(model.ToDoUserId, ct);
        var currentUser = await GetApplicationUser();
        await EnsureAbleTo<ToDoUser>(userId == currentUser.Id ? PermissionType.AssignTasksThemself : PermissionType.ManageAssigners, model.ToDoUserId, ct);
        return await _toDoUserService.RemoveUser(model.ToDoUserId, ct);
    }
    
    [HttpPut("{toDoId:int}")]
    public async Task<List<ToDoDto>> MoveToDo(int toDoId, [FromBody]ToDoMoveModel model, CancellationToken ct)
    {
        await EnsureAbleTo<ToDo>(PermissionType.MoveTasks, toDoId, ct);
        return await _toDoService.MoveToDo(toDoId, model, ct);
    }
    
}
