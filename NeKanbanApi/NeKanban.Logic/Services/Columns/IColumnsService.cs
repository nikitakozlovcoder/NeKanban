﻿using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.Columns;
using NeKanban.Common.Models.ColumnModels;

namespace NeKanban.Logic.Services.Columns;

public interface IColumnsService
{
    public Task<List<ColumnDto>> GetColumns(int deskId, CancellationToken ct);
    public Task<List<ColumnDto>> DeleteColumn(int columnId, CancellationToken ct);
    public Task<List<ColumnDto>> CreateColumn(int deskId, ColumnCreateModel model, ColumnType columnType, CancellationToken ct);
    public Task<List<ColumnDto>> UpdateColumn(int columnId, ColumnUpdateModel model, CancellationToken ct);
    public Task<List<ColumnDto>> MoveColumn(int columnId, ColumnMoveModel model, CancellationToken ct);
}