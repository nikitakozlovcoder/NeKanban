using Batteries.Mapper.Interfaces;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.Columns;

namespace NeKanban.Common.ViewModels.Columns;

public class ColumnVm : BaseEntityModel<int>, IMapFrom<ColumnDto, ColumnVm>
{
    public required string? Name { get; set; }
    public required ColumnType Type { get; set; }
    public string Typename => Type.ToString();
    public required int Order { get; set; }
}