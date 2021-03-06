using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeKanban.Constants;
using NeKanban.Data;
using NeKanban.Data.Entities;
using NeKanban.ExceptionHandling;
using NeKanban.Mappings;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.ToDos.ToDoUsers;

public class ToDoUserService : BaseService, IToDoUserService
{
    private readonly IRepository<ToDoUser> _toDoUserRepository;
    private readonly IToDoService _toDoService;
    public ToDoUserService(
        IRepository<ToDoUser> toDoUserRepository,
        UserManager<ApplicationUser> userManager,
        IToDoService toDoService) : base(userManager)
    {
        _toDoUserRepository = toDoUserRepository;
        _toDoService = toDoService;
    }

    public async Task<ToDoVm> AssignUser(int todoId, int deskUserId, CancellationToken ct)
    {
        var exists = await _toDoUserRepository.QueryableSelect()
            .AnyAsync(x => x.ToDoId == todoId && x.DeskUserId == deskUserId && x.ToDoUserType != ToDoUserType.Creator, ct);
        
        if (exists)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "User already assigned");
        }
      
        var toDoUser = new ToDoUser
        {
            DeskUserId = deskUserId,
            ToDoId = todoId,
            ToDoUserType = ToDoUserType.Assignee
        };
        await _toDoUserRepository.Create(toDoUser, ct);
        return (await _toDoService.GetToDo(todoId, ct))!.ToToDoVm();
    }

    public async Task<ToDoVm> RemoveUser(int toDoUserId, CancellationToken ct)
    {
        var toDoUser = await _toDoUserRepository.QueryableSelect()
            .FirstOrDefaultAsync(x => x.Id == toDoUserId, ct);
        EnsureEntityExists(toDoUser);
        if (toDoUser!.ToDoUserType == ToDoUserType.Creator)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Can`t remove creator");
        }

        await _toDoUserRepository.Remove(toDoUser, ct);
        return (await _toDoService.GetToDo(toDoUser.ToDoId, ct))!.ToToDoVm();
    }

    public async Task<int> GetToDoUserUserId(int toDoUserId, CancellationToken ct)
    {
        var todoUser = await _toDoUserRepository.QueryableSelect()
            .Include(x=> x.DeskUser)
            .FirstOrDefaultAsync(x => x.Id == toDoUserId, ct);
        EnsureEntityExists(todoUser);
        return todoUser!.DeskUser!.UserId;
    }
}