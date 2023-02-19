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
using NeKanban.Common.Models.ToDoModels;
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
    private readonly IAppMapper _mapper;

    public ToDoService(
        IRepository<Desk> deskRepository, 
        IColumnsService columnsService, 
        IRepository<ToDo> toDoRepository, 
        IRepository<DeskUser> deskUserRepository, 
        IRepository<ToDoUser> toDoUserRepository,
        IRepository<Column> columnRepository,
        IAppMapper mapper)
    {
        _deskRepository = deskRepository;
        _columnsService = columnsService;
        _toDoRepository = toDoRepository;
        _deskUserRepository = deskUserRepository;
        _toDoUserRepository = toDoUserRepository;
        _columnRepository = columnRepository;
        _mapper = mapper;
    }

    public async Task<List<ToDoDto>> GetToDos(int deskId, CancellationToken ct)
    {
        await _deskRepository.AnyOrThrow(x=> x.Id == deskId, ct);
        var todos = await _toDoRepository.ProjectTo<ToDoDto>(x => x.Column!.DeskId == deskId, ct);
        return todos;
    }

    public async Task<List<ToDoDto>> CreateToDo(int deskId, ApplicationUser user, ToDoCreateModel model, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.FirstOrDefault(x => x.UserId == user.Id && x.DeskId == deskId, ct); 
        EnsureEntityExists(deskUser);
        var todo = _mapper.AutoMap<ToDo, ToDoCreateModel>(model);
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


    public async Task<List<ToDoDto>> MoveToDo(int toDoId, ToDoMoveModel model, CancellationToken ct)
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

    public async Task<ToDoDto> UpdateToDo(int toDoId, ToDoUpdateModel model, CancellationToken ct)
    {
        var todo = await _toDoRepository.Single(x => x.Id == toDoId, ct);
        _mapper.AutoMap(model, todo);
        await _toDoRepository.Update(todo, ct);
        return _mapper.AutoMap<ToDoDto, ToDo>(todo!);
    }

    public Task<ToDoDto> GetToDo(int toDoId, CancellationToken ct)
    {
        return _toDoRepository.ProjectToSingle<ToDoDto>(x => x.Id == toDoId, ct);
    }
}