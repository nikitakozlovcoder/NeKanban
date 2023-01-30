using NeKanban.Common.Entities;
using NeKanban.Common.Models.ToDoModels;
using NeKanban.Common.ViewModels;

namespace NeKanban.Logic.Services.ToDos;

public interface IToDoService
{
    public Task<List<ToDoVm>> GetToDos(int deskId, CancellationToken ct);
    public Task<List<ToDoVm>> CreateToDo(int deskId, ApplicationUser user, ToDoCreateModel model, CancellationToken ct);
    public Task<List<ToDoVm>> MoveToDo(int toDoId, ToDoMoveModel model,  CancellationToken ct);
    public Task<ToDoVm> UpdateToDo(int toDoId, ToDoUpdateModel model,  CancellationToken ct);
    public Task<ToDo?> GetToDo(int toDoId, CancellationToken ct);
}