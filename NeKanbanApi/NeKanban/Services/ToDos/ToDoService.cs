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

    public ToDoService(IRepository<Desk> deskRepository, 
        UserManager<ApplicationUser> userManager, IColumnsService columnsService, IRepository<ToDo> toDoRepository) : base(userManager)
    {
        _deskRepository = deskRepository;
        _columnsService = columnsService;
        _toDoRepository = toDoRepository;
    }

    public async Task<List<ToDoVm>> GetToDos(int deskId, CancellationToken ct)
    {
        var desk = await _deskRepository.QueryableSelect()
            .Include(x=> x.Columns)
            .ThenInclude(x=> x.ToDos)
            .ThenInclude(x=> x.ToDoUsers)
            .ThenInclude(x=> x.DeskUser)
            .ThenInclude(x=> x!.User)
            .SingleOrDefaultAsync(x => x.Id == deskId, ct);
        
        EnsureEntityExists(desk);
        var todos = desk!.Columns.SelectMany(x => x.ToDos);
        return todos.Select(x => x.ToToDoVm()).ToList();
    }

    public async Task<List<ToDoVm>> CreateToDo(int deskId, ApplicationUser user, ToDoCreateModel model, CancellationToken ct)
    {
        var todo = new ToDo();
        todo.FromCreateModel(model);
        var columns = await _columnsService.GetColumns(deskId, ct);
        todo.ColumnId =  columns.Single(x => x.Type == ColumnType.Start).Id;
        await _toDoRepository.Create(todo, ct);
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