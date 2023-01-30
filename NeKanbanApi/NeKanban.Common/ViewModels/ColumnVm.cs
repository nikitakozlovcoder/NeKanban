using AutoMapper;
using NeKanban.Common.Constants;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.ViewModels;

public class ColumnVm : BaseIdVm, IMapFrom<Column, ColumnVm>
{
    public string? Name { get; set; }
    public ColumnType Type { get; set; }
    public string Typename => Type.ToString();
    public int Order { get; set; }
    public static void ConfigureMap(IMappingExpression<Column, ColumnVm> cfg)
    {
    }
}