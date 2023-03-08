using Batteries.Injection.Attributes;
using Batteries.Mapper;
using Batteries.Mapper.Interfaces;
using JetBrains.Annotations;
using NeKanban.Common.DTOs.Columns;
using NeKanban.Common.ViewModels.Columns;

namespace NeKanban.Logic.Mappings.Columns;

[UsedImplicitly]
[Injectable<IMappingProfile<ColumnDto, ColumnVm>>]
public class ColumnVmMapping : BaseMappingProfile<ColumnDto, ColumnVm>
{
    public override Task<ColumnVm> Map(ColumnDto source, CancellationToken ct)
    {
        return Task.FromResult(new ColumnVm
        {
            Name = source.Name,
            Type = source.Type,
            Order = source.Order,
            Id = source.Id
        });
    }
}