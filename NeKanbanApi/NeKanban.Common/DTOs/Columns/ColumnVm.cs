using Batteries.Mapper.Interfaces;
using NeKanban.Common.Constants;
using NeKanban.Common.ViewModels.Columns;

namespace NeKanban.Common.DTOs.Columns;

public class ColumnVm : BaseEntityModel<int>, IMapFrom<ColumnDto, ColumnVm>
{
    public required string? Name { get; set; }
    public required ColumnType Type { get; set; }
    public string Typename => Type.ToString();
    public required int Order { get; set; }
}