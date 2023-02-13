using NeKanban.Common.DTOs.ToDos;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.ToDoModels;

namespace NeKanban.Logic.Services.ToDos;

public interface IToDoService
{
    public Task<List<ToDoDto>> GetToDos(int deskId, CancellationToken ct);
    public Task<List<ToDoDto>> CreateToDo(int deskId, ApplicationUser user, ToDoCreateModel model, CancellationToken ct);
    public Task<List<ToDoDto>> MoveToDo(int toDoId, ToDoMoveModel model,  CancellationToken ct);
    public Task<ToDoDto> UpdateToDo(int toDoId, ToDoUpdateModel model,  CancellationToken ct);
    public Task<ToDoDto> GetToDo(int toDoId, CancellationToken ct);
}