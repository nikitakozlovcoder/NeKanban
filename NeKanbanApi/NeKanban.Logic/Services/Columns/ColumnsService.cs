using System.Net;
using Batteries.Exceptions;
using Batteries.Injection.Attributes;
using Batteries.Mapper.AppMapper;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.Columns;
using NeKanban.Common.Entities;
using NeKanban.Common.Exceptions;
using NeKanban.Common.Models.ColumnModels;
using NeKanban.Common.ViewModels.Columns;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.Services.Columns;

[UsedImplicitly]
[Injectable<IColumnsService>]
public class ColumnsService : BaseService, IColumnsService
{
    private readonly IRepository<Column> _columnRepository;
    private readonly IAppMapper _mapper;
    private readonly IRepository<Desk> _deskRepository;
    public ColumnsService(
        IRepository<Column> columnRepository, 
        IRepository<Desk> deskRepository,
        IAppMapper mapper)
    {
        _columnRepository = columnRepository;
        _deskRepository = deskRepository;
        _mapper = mapper;
    }

    public async Task<List<ColumnDto>> GetColumns(int deskId, CancellationToken ct)
    {
        var columns = await _columnRepository.ProjectTo<ColumnDto>(x => x.DeskId == deskId, ct);
        return columns;
    }

    public async Task<List<ColumnDto>> DeleteColumn(int columnId, CancellationToken ct)
    {
        var column = await _columnRepository.First(x => x.Id == columnId, ct);
        if (column.Type is ColumnType.End or ColumnType.Start)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, Exceptions.CantDeleteColumnWithThisType);
        }
        
        var deskId = column.DeskId;
        await _columnRepository.Remove(column, ct);
        return await GetColumns(deskId, ct);
    }

    public async Task<List<ColumnDto>> CreateColumn(int deskId, ColumnCreateModel model, ColumnType columnType, CancellationToken ct)
    {
        await _deskRepository.AnyOrThrow(x => x.Id == deskId, ct);
        var column = _mapper.AutoMap<Column, ColumnCreateModel>(model);
        column.Order = GetColumnOrder(columnType);
        column.DeskId = deskId;
        column.Type = columnType;
        await _columnRepository.Create(column, ct);
        if (columnType is ColumnType.End or ColumnType.Start)
        {
            return await GetColumns(deskId, ct);
        }
        
        return await MoveColumn(column.Id, new ColumnMoveModel
        {
            Position = 0
        }, ct);
    }

    public async Task<List<ColumnDto>> UpdateColumn(int columnId, ColumnUpdateModel model, CancellationToken ct)
    {
        var column = await _columnRepository.Single(x => x.Id == columnId, ct);
        _mapper.AutoMap(model, column);
        await _columnRepository.Update(column, ct);
        return await GetColumns(column.DeskId, ct);
    }

    public async Task<List<ColumnDto>> MoveColumn(int columnId, ColumnMoveModel model, CancellationToken ct)
    {
        var column = await _columnRepository.First(x=> x.Id == columnId, ct);
        var columns = await _columnRepository.ToList(x => x.DeskId == column!.DeskId, ct);
        var positionToMove = model.Position;
        foreach (var columnItem in columns.OrderBy(x=> x.Order))
        {
            if (columnItem.Id == column.Id)
            {
                if (columnItem.Order == model.Position) continue;
                columnItem.Order = model.Position;
                await _columnRepository.Update(columnItem, ct);
            }
            else if (columnItem.Order == positionToMove)
            {
                columnItem.Order = ++positionToMove;
                await _columnRepository.Update(columnItem, ct);
            }
        }

        return await GetColumns(column.DeskId, ct);
    }

    private static int GetColumnOrder(ColumnType columnType)
    {
        return columnType switch
        {
            ColumnType.Start => -1,
            ColumnType.General => 0,
            ColumnType.End => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(columnType), columnType, null)
        };
    }
}