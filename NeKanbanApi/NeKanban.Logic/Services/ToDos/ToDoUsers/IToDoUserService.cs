using NeKanban.Logic.Services.ViewModels;

namespace NeKanban.Services.ToDos.ToDoUsers;

public interface IToDoUserService
{
    Task<ToDoVm> AssignUser(int todoId, int deskUserId, CancellationToken ct);
    Task<ToDoVm> RemoveUser(int toDoUserId, CancellationToken ct);
    Task<int> GetToDoUserUserId(int toDoUserId, CancellationToken ct);
}