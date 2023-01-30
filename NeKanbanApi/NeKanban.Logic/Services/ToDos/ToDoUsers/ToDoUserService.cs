using System.Net;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NeKanban.Api.FrameworkExceptions.ExceptionHandling;
using NeKanban.Common.Attributes;
using NeKanban.Common.Constants;
using NeKanban.Common.Entities;
using NeKanban.Common.ViewModels;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.Services.ToDos.ToDoUsers;

[UsedImplicitly]
[Injectable<IToDoUserService>]
public class ToDoUserService : BaseService, IToDoUserService
{
    private readonly IRepository<ToDoUser> _toDoUserRepository;
    private readonly IToDoService _toDoService;
    private readonly IMapper _mapper;
    public ToDoUserService(
        IRepository<ToDoUser> toDoUserRepository,
        IToDoService toDoService,
        IMapper mapper)
    {
        _toDoUserRepository = toDoUserRepository;
        _toDoService = toDoService;
        _mapper = mapper;
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
        var todo = await _toDoService.GetToDo(todoId, ct);
        return _mapper.Map<ToDoVm>(todo);
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
        var todo = await _toDoService.GetToDo(toDoUser.ToDoId, ct);
        return _mapper.Map<ToDoVm>(todo);
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