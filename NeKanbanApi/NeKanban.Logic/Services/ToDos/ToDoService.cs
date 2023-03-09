using System.Net;
using Batteries.Exceptions;
using Batteries.FileStorage.FileStorageAdapters;
using Batteries.FileStorage.FileStorageProxies;
using Batteries.Injection.Attributes;
using Batteries.Mapper.AppMapper;
using Batteries.Repository;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.ToDos;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.ToDoModels;
using NeKanban.Common.ViewModels.ToDos;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Services.Columns;

namespace NeKanban.Logic.Services.ToDos;

[UsedImplicitly]
[Injectable<IToDoService>]
public class ToDoService : BaseService, IToDoService
{
    private readonly IRepository<Desk> _deskRepository;
    private readonly IRepository<ToDo> _toDoRepository;
    private readonly IColumnsService _columnsService;
    private readonly IRepository<DeskUser> _deskUserRepository;
    private readonly IRepository<ToDoUser> _toDoUserRepository;
    private readonly IRepository<Column> _columnRepository;
    private readonly IAppMapper _mapper;
    private readonly IFileStorageAdapter<ToDoFileAdapter, ToDo> _toDoFileStorageAdapter;
    private readonly IFileStorageProxy _fileStorageProxy;
    private readonly QueryFilterSettings _filterSettings;

    public ToDoService(
        IRepository<Desk> deskRepository, 
        IColumnsService columnsService, 
        IRepository<ToDo> toDoRepository, 
        IRepository<DeskUser> deskUserRepository, 
        IRepository<ToDoUser> toDoUserRepository,
        IRepository<Column> columnRepository,
        IAppMapper mapper,
        IFileStorageAdapter<ToDoFileAdapter, ToDo> toDoFileStorageAdapter,
        IFileStorageProxy fileStorageProxy, QueryFilterSettings filterSettings)
    {
        _deskRepository = deskRepository;
        _columnsService = columnsService;
        _toDoRepository = toDoRepository;
        _deskUserRepository = deskUserRepository;
        _toDoUserRepository = toDoUserRepository;
        _columnRepository = columnRepository;
        _mapper = mapper;
        _toDoFileStorageAdapter = toDoFileStorageAdapter;
        _fileStorageProxy = fileStorageProxy;
        _filterSettings = filterSettings;
    }

    public async Task<List<ToDoDto>> GetToDos(int deskId, CancellationToken ct)
    {
        await _deskRepository.AnyOrThrow(x=> x.Id == deskId, ct);
        var todos = await _toDoRepository.ProjectTo<ToDoDto>(x => x.Column!.DeskId == deskId, ct);
        return todos;
    }

    public async Task<ToDoFullDto> GetToDoFull(int todoId, CancellationToken ct)
    {
        var dto = await _toDoRepository.ProjectToSingle<ToDoFullDto>(x => x.Id == todoId, ct);
        return dto;
    }
    
    public async Task<ToDoDraftDto> GetDraft(int deskId, ApplicationUser user, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions()
        {
            ToDoDraftFilter = false
        });
        
        var deskUser = await _deskUserRepository.First(x => x.UserId == user.Id && x.DeskId == deskId, ct);
        var columns = await _columnsService.GetColumns(deskId, ct);
        var column = columns.Single(x => x.Type == ColumnType.Start).Id;
        var draft = await _toDoRepository.ProjectToFirstOrDefault<ToDoDraftDto>(x =>
            x.IsDraft &&
            x.ColumnId == column &&
            x.ToDoUsers.Any(u => u.ToDoUserType == ToDoUserType.Creator && u.DeskUserId == deskUser.Id), ct);

        if (draft != null)
        {
            return draft;
        }
        
        var todo = new ToDo
        {
            Order = 0,
            Name = string.Empty,
            Body = null,
            ColumnId = column,
            IsDraft = true
        };
        
        await _toDoRepository.Create(todo, ct);
        var creator = new ToDoUser
        {
            ToDoUserType = ToDoUserType.Creator,
            ToDoId = todo.Id,
            DeskUserId = deskUser.Id
        };
        
        await _toDoUserRepository.Create(creator, ct);
        return _mapper.AutoMap<ToDoDraftDto, ToDo>(todo);
    }
    
    public async Task<ToDoDraftDto> UpdateDraftToDo(int toDoId, ApplicationUser user, ToDoUpdateModel model,
        CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            ToDoDraftFilter = false
        });
        await _toDoUserRepository.AnyOrThrow(x => x.DeskUser!.UserId == user.Id && x.ToDoId == toDoId, ct);
        var todo = await _toDoRepository.Single(x => x.Id == toDoId && x.IsDraft, ct);
        _mapper.AutoMap(model, todo);
        await _toDoRepository.Update(todo, ct);
        return _mapper.AutoMap<ToDoDraftDto, ToDo>(todo);
    }

    public async Task<ToDoFullDto> ApplyDraftToDo(int toDoId, ApplicationUser user, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            ToDoDraftFilter = false
        });
        await _toDoUserRepository.AnyOrThrow(x => x.DeskUser!.UserId == user.Id && x.ToDoId == toDoId, ct);
        var todo = await _toDoRepository.Single(x => x.Id == toDoId && x.IsDraft, ct);
        todo.IsDraft = false;
        todo.Order = await GetCreateOrderInColum(todo.ColumnId, ct);
        await _toDoRepository.Update(todo, ct);
        return await GetToDoFull(todo.Id, ct);
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
        return _mapper.AutoMap<ToDoDto, ToDo>(todo);
    }

    public Task<ToDoDto> GetToDo(int toDoId, CancellationToken ct)
    {
        return _toDoRepository.ProjectToSingle<ToDoDto>(x => x.Id == toDoId, ct);
    }

    public async Task<string> AttachFile(int toDoId, IFormFile file, CancellationToken ct)
    {
        var entity = await _toDoFileStorageAdapter.Store(toDoId, file, ct);
        return _fileStorageProxy.GetProxyUrl(entity.FileName);
    }
}