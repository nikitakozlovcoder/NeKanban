using NeKanban.Controllers.Models.ToDoModels;
using NeKanban.Data.Entities;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.ToDos;

public interface IToDoService
{
    public Task<List<ToDoVm>> GetToDos(int deskId, CancellationToken ct);
    public Task<List<ToDoVm>> CreateToDo(int deskId, ApplicationUser user, ToDoCreateModel model, CancellationToken ct);
    public Task<List<ToDoVm>> MoveToDo(int toDoId, ToDoMoveModel model,  CancellationToken ct);
    public Task<ToDoVm> UpdateToDo(int toDoId, ToDoUpdateModel model,  CancellationToken ct);

    public Task<ToDo?> GetToDo(int toDoId, CancellationToken ct);
}