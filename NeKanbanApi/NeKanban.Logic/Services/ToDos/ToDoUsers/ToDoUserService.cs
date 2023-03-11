using System.Net;
using Batteries.Exceptions;
using Batteries.Injection.Attributes;
using Batteries.Mapper.AppMapper;
using Batteries.Repository;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.ToDos;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.Services.ToDos.ToDoUsers;

[UsedImplicitly]
[Injectable<IToDoUserService>]
public class ToDoUserService : IToDoUserService
{
    private readonly IRepository<ToDoUser> _toDoUserRepository;
    private readonly IToDoService _toDoService;
    private readonly IAppMapper _mapper;
    public ToDoUserService(
        IRepository<ToDoUser> toDoUserRepository,
        IToDoService toDoService,
        IAppMapper mapper)
    {
        _toDoUserRepository = toDoUserRepository;
        _toDoService = toDoService;
        _mapper = mapper;
    }

    public async Task<ToDoDto> AssignUser(int todoId, int deskUserId, CancellationToken ct)
    {
        var exists = await _toDoUserRepository
            .Any(x => x.ToDoId == todoId
                      && x.DeskUserId == deskUserId 
                      && x.ToDoUserType != ToDoUserType.Creator, ct);
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
        return todo;
    }

    public async Task<ToDoDto> RemoveUser(int toDoUserId, CancellationToken ct)
    {
        var toDoUser = await _toDoUserRepository.Single(x => x.Id == toDoUserId, ct);
        if (toDoUser.ToDoUserType == ToDoUserType.Creator)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Can`t remove creator");
        }

        await _toDoUserRepository.Remove(toDoUser, ct);
        var todo = await _toDoService.GetToDo(toDoUser.ToDoId, ct);
        return todo;
    }

    public async Task<int> GetToDoUserUserId(int toDoUserId, CancellationToken ct)
    {
        var userId = await _toDoUserRepository.Single(x => x.Id == toDoUserId, x => x.DeskUser!.UserId, ct);
        return userId;
    }
}