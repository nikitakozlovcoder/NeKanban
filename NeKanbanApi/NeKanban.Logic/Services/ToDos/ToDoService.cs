using System.Net;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NeKanban.Api.FrameworkExceptions.ExceptionHandling;
using NeKanban.Common.Attributes;
using NeKanban.Common.Constants;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.ToDoModels;
using NeKanban.Common.ViewModels;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Services.Columns;

namespace NeKanban.Logic.Services.ToDos;

[UsedImplicitly]
[Injectable(typeof(IToDoService))]
public class ToDoService : BaseService, IToDoService
{
    private readonly IRepository<Desk> _deskRepository;
    private readonly IRepository<ToDo> _toDoRepository;
    private readonly IColumnsService _columnsService;
    private readonly IRepository<DeskUser> _deskUserRepository;
    private readonly IRepository<ToDoUser> _toDoUserRepository;
    private readonly IRepository<Column> _columnRepository;
    private readonly IMapper _mapper;

    public ToDoService(
        IRepository<Desk> deskRepository, 
        IColumnsService columnsService, 
        IRepository<ToDo> toDoRepository, 
        IRepository<DeskUser> deskUserRepository, 
        IRepository<ToDoUser> toDoUserRepository,
        IRepository<Column> columnRepository,
        IMapper mapper)
    {
        _deskRepository = deskRepository;
        _columnsService = columnsService;
        _toDoRepository = toDoRepository;
        _deskUserRepository = deskUserRepository;
        _toDoUserRepository = toDoUserRepository;
        _columnRepository = columnRepository;
        _mapper = mapper;
    }

    public async Task<List<ToDoVm>> GetToDos(int deskId, CancellationToken ct)
    {
        var desk = await _deskRepository.QueryableSelect().FirstOrDefaultAsync(x=> x.Id == deskId, cancellationToken: ct);
        EnsureEntityExists(desk);
        var todos = await _toDoRepository.QueryableSelect()
                .Include(x=> x.ToDoUsers)
                .ThenInclude(x=> x.DeskUser)
                .ThenInclude(x=> x!.User)
                .Include(x=> x.Column)
                .Where(x => x.Column!.DeskId == deskId).ToListAsync(ct);
        
        return _mapper.Map<List<ToDoVm>>(todos);
    }

    public async Task<List<ToDoVm>> CreateToDo(int deskId, ApplicationUser user, ToDoCreateModel model, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.QueryableSelect()
            .FirstOrDefaultAsync(x => x.UserId == user.Id && x.DeskId == deskId, ct); 
        EnsureEntityExists(deskUser);
        var todo = _mapper.Map<ToDo>(model);
        var columns = await _columnsService.GetColumns(deskId, ct);
        todo.ColumnId = columns.Single(x => x.Type == ColumnType.Start).Id;
        todo.Order = await GetCreateOrderInColum(todo.ColumnId, ct);
        await _toDoRepository.Create(todo, ct);

        var creator = new ToDoUser
        {
            ToDoUserType = ToDoUserType.Creator,
            ToDoId = todo.Id,
            DeskUserId = deskUser!.Id
        };
        await _toDoUserRepository.Create(creator, ct);
        return await GetToDos(deskId, ct);
    }

    private async Task<int> GetCreateOrderInColum(int todoColumnId, CancellationToken ct)
    {
        var minId = await _toDoRepository.QueryableSelect().Where(x => x.ColumnId == todoColumnId)
            .MinAsync(x => (int?)x.Order, ct) ?? 0;
        return Math.Min(minId, 0) - 1;
    }


    public async Task<List<ToDoVm>> MoveToDo(int toDoId, ToDoMoveModel model, CancellationToken ct)
    {
        var toDo = await _toDoRepository.QueryableSelect().Include(x=> x.Column)
            .FirstOrDefaultAsync(x => x.Id == toDoId, ct);
        EnsureEntityExists(toDo);
        var isMoveValid = await _columnRepository.QueryableSelect()
            .AnyAsync(x => x.Id == model.ColumnId && x.DeskId == toDo!.Column!.DeskId, ct);
        if (!isMoveValid)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest);
        }
        var others = await _toDoRepository.QueryableSelect()
            .Where(x => x.ColumnId == model.ColumnId && x.Id != toDoId && x.Order >= model.Order).OrderBy(x=> x.Order).ToListAsync(ct);
        var order = model.Order;
        toDo!.ColumnId = model.ColumnId;
        toDo.Order = order;
        
        foreach (var item in others)
        {
            if (item.Order == order)
            {
                item.Order = ++order;
                await _toDoRepository.Update(item, ct);
            }
            else
            {
                break;
            }
        }
        
        await _toDoRepository.Update(toDo, ct);
        return await GetToDos(toDo.Column!.DeskId, ct);
    }

    public async Task<ToDoVm> UpdateToDo(int toDoId, ToDoUpdateModel model, CancellationToken ct)
    {
        var todo = await GetToDo(toDoId, ct);
        EnsureEntityExists(todo);
        _mapper.Map(model, todo);
        await _toDoRepository.Update(todo!, ct);
        return _mapper.Map<ToDoVm>(todo);
    }

    public Task<ToDo?> GetToDo(int toDoId, CancellationToken ct)
    {
        return _toDoRepository.QueryableSelect().Include(x => x.ToDoUsers)
            .ThenInclude(x => x.DeskUser)
            .ThenInclude(x => x!.User)
            .Include(x => x.Column)
            .FirstOrDefaultAsync(x => x.Id == toDoId, ct);
    }
}