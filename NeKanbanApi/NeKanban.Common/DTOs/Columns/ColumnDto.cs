using AutoMapper;
using NeKanban.Common.Constants;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.DTOs.Columns;

public class ColumnDto : BaseEntityDto<int>, IMapFrom<Column, ColumnDto>
{
    public string? Name { get; set; }
    public ColumnType Type { get; set; }
    public int Order { get; set; }
    public static void ConfigureMap(IMappingExpression<Column, ColumnDto> cfg)
    {
    }
}