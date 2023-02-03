using AutoMapper;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.Columns;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class ColumnVm : BaseIdVm, IMapFrom<ColumnDto, ColumnVm>, IMapFrom<Column, ColumnVm>
{
    public required string? Name { get; set; }
    public ColumnType Type { get; set; }
    public string Typename => Type.ToString();
    public int Order { get; set; }
    public static void ConfigureMap(IMappingExpression<Column, ColumnVm> cfg)
    {
    }

    public static void ConfigureMap(IMappingExpression<ColumnDto, ColumnVm> cfg)
    {
    }
}