using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Controllers.Models.ToDoModels;
using NeKanban.Data.Entities;
using NeKanban.Services.ToDos;
using NeKanban.Services.ViewModels;

namespace NeKanban.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ToDosController : BaseAuthController
{
    private readonly IToDoService _toDoService;
    public ToDosController(UserManager<ApplicationUser> userManager, IToDoService toDoService) : base(userManager)
    {
        _toDoService = toDoService;
    }
    
    [HttpGet("{deskId:int}")]
    public async Task<List<ToDoVm>> GetTodos(int deskId, CancellationToken ct)
    {
        return await _toDoService.GetToDos(deskId, ct);
    }
    
    [HttpPost("{deskId:int}")]
    public async Task<List<ToDoVm>> CreateToDo(int deskId, ToDoCreateModel model, CancellationToken ct)
    {
        var user = await GetApplicationUser();
        return await _toDoService.CreateToDo(deskId, user, model, ct);
    }
    
    
}