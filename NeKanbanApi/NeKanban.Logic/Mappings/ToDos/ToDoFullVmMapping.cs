using Batteries.FileStorage.FileStorageAdapters;
using Batteries.Injection.Attributes;
using Batteries.Mapper;
using Batteries.Mapper.AppMapper;
using Batteries.Mapper.Interfaces;
using JetBrains.Annotations;
using NeKanban.Common.DTOs.Columns;
using NeKanban.Common.DTOs.ToDos;
using NeKanban.Common.DTOs.ToDoUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.ViewModels.Columns;
using NeKanban.Common.ViewModels.ToDos;
using NeKanban.Common.ViewModels.ToDoUsers;

namespace NeKanban.Logic.Mappings.ToDos;

[UsedImplicitly]
[Injectable<IMappingProfile<ToDoFullDto, ToDoFullVm>>]
public class ToDoFullVmMapping : BaseMappingProfile<ToDoFullDto, ToDoFullVm>
{
    private readonly IFileStorageAdapter<ToDoFileAdapter, ToDo> _toDoFileStorageAdapter;
    private readonly IAppMapper _mapper;

    public ToDoFullVmMapping(IFileStorageAdapter<ToDoFileAdapter, ToDo> toDoFileStorageAdapter, IAppMapper mapper)
    {
        _toDoFileStorageAdapter = toDoFileStorageAdapter;
        _mapper = mapper;
    }

    public override async Task<ToDoFullVm> Map(ToDoFullDto source, CancellationToken ct)
    {
        var files = await _toDoFileStorageAdapter.GetAllUrls(source.Id, ct);
        var column = await _mapper.Map<ColumnVm, ColumnDto>(source.Column, ct);
        var users = await _mapper.Map<ToDoUserVm, ToDoUserDto>(source.ToDoUsers, ct);
        return new ToDoFullVm
        {
            Id = source.Id,
            Body = source.Body ?? string.Empty,
            Name = source.Name,
            Order = source.Order,
            Column = column,
            ToDoUsers = users,
            Files = files
        };
    }
}