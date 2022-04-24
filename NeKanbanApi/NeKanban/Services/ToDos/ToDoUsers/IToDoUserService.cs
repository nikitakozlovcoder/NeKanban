using NeKanban.Services.ViewModels;

namespace NeKanban.Services.ToDos.ToDoUsers;

public interface IToDoUserService
{
    Task<ToDoVm> AssignUser(int todoId, int deskUserId, CancellationToken ct);
    Task<ToDoVm> RemoveUser(int toDoUserId, CancellationToken ct);
}