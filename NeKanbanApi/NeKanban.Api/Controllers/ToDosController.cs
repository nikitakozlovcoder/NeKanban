using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Controllers.Auth;
using NeKanban.Data.Entities;
using NeKanban.Logic.Models.ToDoModels;
using NeKanban.Logic.Services.DesksUsers;
using NeKanban.Logic.Services.ToDos;
using NeKanban.Logic.Services.ViewModels;
using NeKanban.Security.Constants;
using NeKanban.Services.ToDos;
using NeKanban.Services.ToDos.ToDoUsers;

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
    public async Task<List<ToDoVm>> GetTodos(int deskId, CancellationToken ct)
    {
        await EnsureAbleTo<Desk>(PermissionType.AccessDesk, deskId, ct);
        return await _toDoService.GetToDos(deskId, ct);
    }
    
    [HttpPost("{deskId:int}")]
    public async Task<List<ToDoVm>> CreateToDo(int deskId, [FromBody]ToDoCreateModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Desk>(PermissionType.CreateTasks, deskId, ct);
        var user = await GetApplicationUser();
        return await _toDoService.CreateToDo(deskId, user, model, ct);
    }
    
    [HttpPut("{toDoId:int}")]
    public async Task<ToDoVm> UpdateToDo(int toDoId, [FromBody]ToDoUpdateModel model, CancellationToken ct)
    {
        await EnsureAbleTo<ToDo>(PermissionType.UpdateTask, toDoId, ct);
        return await _toDoService.UpdateToDo(toDoId, model, ct);
    }
    
    [HttpPut("{toDoId:int}")]
    public async Task<ToDoVm> AssignUser(int toDoId, [FromBody]AssignUserModel model, CancellationToken ct)
    {
        var userId= await _deskUserService.GetDeskUserUserId(model.DescUserId, ct);
        var currentUser = await GetApplicationUser();
        await EnsureAbleTo<ToDo>(userId == currentUser.Id ? PermissionType.AssignTasksThemself : PermissionType.ManageAssigners, toDoId, ct);
        return await _toDoUserService.AssignUser(toDoId, model.DescUserId, ct);
    }
    
    [HttpDelete]
    public async Task<ToDoVm> RemoveUser([FromBody]RemoveUserModel model, CancellationToken ct)
    {
        var userId= await _toDoUserService.GetToDoUserUserId(model.ToDoUserId, ct);
        var currentUser = await GetApplicationUser();
        await EnsureAbleTo<ToDoUser>(userId == currentUser.Id ? PermissionType.AssignTasksThemself : PermissionType.ManageAssigners, model.ToDoUserId, ct);
        return await _toDoUserService.RemoveUser(model.ToDoUserId, ct);
    }
    
    [HttpPut("{toDoId:int}")]
    public async Task<List<ToDoVm>> MoveToDo(int toDoId, [FromBody]ToDoMoveModel model, CancellationToken ct)
    {
        await EnsureAbleTo<ToDo>(PermissionType.MoveTasks, toDoId, ct);
        return await _toDoService.MoveToDo(toDoId, model, ct);
    }
    
}
