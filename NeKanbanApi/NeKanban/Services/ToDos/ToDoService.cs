using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeKanban.Constants;
using NeKanban.Controllers.Models.ToDoModels;
using NeKanban.Data;
using NeKanban.Data.Entities;
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

    public ToDoService(
        IRepository<Desk> deskRepository, 
        UserManager<ApplicationUser> userManager, 
        IColumnsService columnsService, 
        IRepository<ToDo> toDoRepository, 
        IRepository<DeskUser> deskUserRepository, 
        IRepository<ToDoUser> toDoUserRepository) : base(userManager)
    {
        _deskRepository = deskRepository;
        _columnsService = columnsService;
        _toDoRepository = toDoRepository;
        _deskUserRepository = deskUserRepository;
        _toDoUserRepository = toDoUserRepository;
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


    public Task<List<ToDoVm>> MoveToDo(int toDoId, ToDoMoveModel model, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<List<ToDoVm>> UpdateToDo(int toDoId, ToDoUpdateModel model, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}