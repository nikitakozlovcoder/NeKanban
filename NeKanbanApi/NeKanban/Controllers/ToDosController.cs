using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Controllers.Models.ToDoModels;
using NeKanban.Data.Entities;
using NeKanban.Services.ToDos;
using NeKanban.Services.ToDos.ToDoUsers;
using NeKanban.Services.ViewModels;

namespace NeKanban.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class ToDosController : BaseAuthController
{
    private readonly IToDoService _toDoService;
    private readonly IToDoUserService _toDoUserService;
    public ToDosController(UserManager<ApplicationUser> userManager,
        IToDoService toDoService,
        IToDoUserService toDoUserService) : base(userManager)
    {
        _toDoService = toDoService;
        _toDoUserService = toDoUserService;
    }
    
    [HttpGet("{deskId:int}")]
    public async Task<List<ToDoVm>> GetTodos(int deskId, CancellationToken ct)
    {
        return await _toDoService.GetToDos(deskId, ct);
    }
    
    [HttpPost("{deskId:int}")]
    public async Task<List<ToDoVm>> CreateToDo(int deskId, [FromBody]ToDoCreateModel model, CancellationToken ct)
    {
        var user = await GetApplicationUser();
        return await _toDoService.CreateToDo(deskId, user, model, ct);
    }
    
    [HttpPut("{toDoId:int}")]
    public async Task<ToDoVm> UpdateToDo(int toDoId, [FromBody]ToDoUpdateModel model, CancellationToken ct)
    {
        return await _toDoService.UpdateToDo(toDoId, model, ct);
    }
    
    [HttpPut("{toDoId:int}")]
    public async Task<ToDoVm> AssignUser(int toDoId, [FromBody]AssignUserModel model, CancellationToken ct)
    {
        return await _toDoUserService.AssignUser(toDoId, model.DescUserId, ct);
    }
    
    [HttpDelete]
    public async Task<ToDoVm> RemoveUser([FromBody]RemoveUserModel model, CancellationToken ct)
    {
        return await _toDoUserService.RemoveUser(model.ToDoUserId, ct);
    }
    
    [HttpPut("{toDoId:int}")]
    public async Task<List<ToDoVm>> MoveToDo(int toDoId, [FromBody]ToDoMoveModel model, CancellationToken ct)
    {
        return await _toDoService.MoveToDo(toDoId, model, ct);
    }
    
}