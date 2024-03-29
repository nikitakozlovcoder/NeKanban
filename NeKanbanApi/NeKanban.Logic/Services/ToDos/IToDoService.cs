﻿using Microsoft.AspNetCore.Http;
using NeKanban.Common.DTOs.ToDos;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.ToDoModels;

namespace NeKanban.Logic.Services.ToDos;

public interface IToDoService
{
    public Task<List<ToDoDto>> GetToDos(int deskId, CancellationToken ct);
    Task<ToDoFullDto> GetToDoFull(int todoId, CancellationToken ct);
    
    public Task<ToDoDraftDto> GetDraft(int deskId, ApplicationUser user, CancellationToken ct);
    public Task<ToDoDraftDto> UpdateDraftToDo(int toDoId, ApplicationUser user, ToDoUpdateModel model,
        CancellationToken ct);
    public Task<ToDoFullDto> ApplyDraftToDo(int toDoId, ApplicationUser user, CancellationToken ct);
    
    public Task<List<ToDoDto>> MoveToDo(int toDoId, ToDoMoveModel model,  CancellationToken ct);
    public Task<ToDoDto> UpdateToDo(int toDoId, ToDoUpdateModel model,  CancellationToken ct);
    public Task<ToDoDto> GetToDo(int toDoId, CancellationToken ct);
    
    Task<string> AttachFile(int toDoId, IFormFile file, CancellationToken ct);
}