﻿using AutoMapper;
using NeKanban.Common.Constants;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.DTOs.Columns;

public class ColumnDto : BaseEntityModel<int>, IAutoMapFrom<Column, ColumnDto>
{
    public required string? Name { get; set; }
    public required ColumnType Type { get; set; }
    public string Typename => Type.ToString();
    public required int Order { get; set; }
    public static void ConfigureMap(IMappingExpression<Column, ColumnDto> cfg)
    {
    }
}