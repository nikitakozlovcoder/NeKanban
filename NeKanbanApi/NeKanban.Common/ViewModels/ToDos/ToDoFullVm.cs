using AutoMapper;
using Batteries.FileStorage.Models;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.DTOs.ToDos;
using NeKanban.Common.ViewModels.Columns;
using NeKanban.Common.ViewModels.ToDoUsers;

namespace NeKanban.Common.ViewModels.ToDos;

public class ToDoFullVm : BaseEntityModel<int>, IMapFrom<ToDoFullDto, ToDoFullVm>
{
    public required string? Name { get; set; }
    public required int Order { get; set; }
    public required ColumnVm Column { get; set; }
    public required List<ToDoUserVm> ToDoUsers { get; set; }
    public required string Body { get; set; }
    public required List<FileStoreUrlDto> Files { get; set; }
    
    public static void ConfigureMap(IMappingExpression<ToDoFullDto, ToDoFullVm> cfg)
    {
    }
}