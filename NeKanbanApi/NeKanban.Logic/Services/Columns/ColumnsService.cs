using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeKanban.Api.FrameworkExceptions.ExceptionHandling;
using NeKanban.Common.Attributes;
using NeKanban.Data.Constants;
using NeKanban.Data.Entities;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Mappings;
using NeKanban.Logic.Models.ColumnModels;
using NeKanban.Logic.Services.ViewModels;

namespace NeKanban.Logic.Services.Columns;

[Injectable<IColumnsService>]
public class ColumnsService : BaseService, IColumnsService
{
    private readonly IRepository<Column> _columnRepository;
    private readonly IRepository<Desk> _deskRepository;
    public ColumnsService(
        UserManager<ApplicationUser> userManager, 
        IRepository<Column> columnRepository, 
        IRepository<Desk> deskRepository) : base(userManager)
    {
        _columnRepository = columnRepository;
        _deskRepository = deskRepository;
    }

    public async Task<List<ColumnVm>> GetColumns(int deskId, CancellationToken ct)
    {
        var columns = await _columnRepository.QueryableSelect()
            .Where(x => x.DeskId == deskId).ToListAsync(ct);
        return columns.Select(x=> x.ToColumnVm()).ToList();
    }

    public async Task<List<ColumnVm>> DeleteColumn(int columnId, CancellationToken ct)
    {
        var column = await _columnRepository.QueryableSelect()
            .FirstOrDefaultAsync(x => x.Id == columnId, ct);
        EnsureEntityExists(column);
        if (column!.Type is ColumnType.End or ColumnType.Start)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, Api.FrameworkExceptions.ExceptionHandling.Exceptions.CantDeleteColumnWithThisType);
        }
        
        var deskId = column!.DeskId;
        await _columnRepository.Remove(column, ct);
        return await GetColumns(deskId, ct);
    }

    public async Task<List<ColumnVm>> CreateColumn(int deskId, ColumnCreateModel model, ColumnType columnType, CancellationToken ct)
    {
        var desk = await _deskRepository.QueryableSelect().FirstOrDefaultAsync(x => x.Id == deskId, ct);
        EnsureEntityExists(desk);
        var column = new Column();
        column.FromCreateModel(model);
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

    public async Task<List<ColumnVm>> UpdateColumn(int columnId, ColumnUpdateModel model, CancellationToken ct)
    {
        var column = await _columnRepository.QueryableSelect().SingleOrDefaultAsync(x => x.Id == columnId, ct);
        EnsureEntityExists(column);
        column!.FromUpdateModel(model);
        await _columnRepository.Update(column!, ct);
        return await GetColumns(column!.DeskId, ct);
    }

    public async Task<List<ColumnVm>> MoveColumn(int columnId, ColumnMoveModel model, CancellationToken ct)
    {
        var column = await _columnRepository.QueryableSelect().FirstOrDefaultAsync(x=> x.Id == columnId, ct); 
        EnsureEntityExists(column);

        var columns = await _columnRepository.QueryableSelect().Where(x => x.DeskId == column!.DeskId).ToListAsync(ct);
        var positionToMove = model.Position;
        foreach (var columnItem in columns.OrderBy(x=> x.Order))
        {
            if (columnItem.Id == column!.Id)
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

        return await GetColumns(column!.DeskId, ct);
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