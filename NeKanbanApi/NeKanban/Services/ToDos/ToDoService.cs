using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeKanban.Constants;
using NeKanban.Controllers.Models.ToDoModels;
using NeKanban.Data;
using NeKanban.Data.Entities;
using NeKanban.ExceptionHandling;
using NeKanban.Mappings;
using NeKanban.Services.Columns;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.ToDos;

public class ToDoService : BaseService, IToDoService
{
    private readonly IRepository<Desk> _deskRepository;
    private readonly IRepository<ToDo> _toDoRepository;
    private readonly IColumnsService _columnsService;
    private readonly IRepository<DeskUser> _deskUserRepository;
    private readonly IRepository<ToDoUser> _toDoUserRepository;
    private readonly IRepository<Column> _columnRepository;

    public ToDoService(
        IRepository<Desk> deskRepository, 
        UserManager<ApplicationUser> userManager, 
        IColumnsService columnsService, 
        IRepository<ToDo> toDoRepository, 
        IRepository<DeskUser> deskUserRepository, 
        IRepository<ToDoUser> toDoUserRepository,
        IRepository<Column> columnRepository) : base(userManager)
    {
        _deskRepository = deskRepository;
        _columnsService = columnsService;
        _toDoRepository = toDoRepository;
        _deskUserRepository = deskUserRepository;
        _toDoUserRepository = toDoUserRepository;
        _columnRepository = columnRepository;
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
        
        return todos.Select(x => x.ToToDoVm()).ToList();
    }

    public async Task<List<ToDoVm>> CreateToDo(int deskId, ApplicationUser user, ToDoCreateModel model, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.QueryableSelect()
            .FirstOrDefaultAsync(x => x.UserId == user.Id && x.DeskId == deskId, ct); 
        EnsureEntityExists(deskUser);
        var todo = new ToDo();
        todo.FromCreateModel(model);
      
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
            .Where(x => x.ColumnId == model.ColumnId && x.Id != toDoId && x.Order >= toDo!.Order).OrderBy(x=> x.Order).ToListAsync(ct);
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
        todo!.FromUpdateModel(model);
        await _toDoRepository.Update(todo!, ct);
        return todo!.ToToDoVm();
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