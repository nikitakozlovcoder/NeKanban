using NeKanban.Common.DTOs.ToDos;

namespace NeKanban.Logic.Services.ToDos.ToDoUsers;

public interface IToDoUserService
{
    Task<ToDoDto> AssignUser(int todoId, int deskUserId, CancellationToken ct);
    Task<ToDoDto> RemoveUser(int toDoUserId, CancellationToken ct);
    Task<int> GetToDoUserUserId(int toDoUserId, CancellationToken ct);
}