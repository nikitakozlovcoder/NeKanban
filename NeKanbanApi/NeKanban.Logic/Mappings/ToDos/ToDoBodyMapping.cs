using Batteries.Injection.Attributes;
using Batteries.Mapper;
using Batteries.Mapper.Interfaces;
using JetBrains.Annotations;
using NeKanban.Common.DTOs.ToDos;
using NeKanban.Common.ViewModels.ToDos;

namespace NeKanban.Logic.Mappings.ToDos;

[UsedImplicitly]
[Injectable<IMappingProfile<ToDoLiteDto, ToDoBodyVm>>]
public class ToDoBodyMapping : BaseMappingProfile<ToDoLiteDto, ToDoBodyVm>
{
    public override async Task<ToDoBodyVm> Map(ToDoLiteDto source, CancellationToken ct)
    {
        return new ToDoBodyVm
        {
            Body = source.Body ?? string.Empty
        };
    }
}