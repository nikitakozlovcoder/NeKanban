using NeKanban.Constants;
using NeKanban.Controllers;
using NeKanban.Controllers.Models.ColumnModels;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.Columns;

public interface IColumnsService
{
    public Task<List<ColumnVm>> GetColumns(int deskId, CancellationToken ct);
    public Task<List<ColumnVm>> DeleteColumn(int columnId, CancellationToken ct);
    public Task<List<ColumnVm>> CreateColumn(int deskId, ColumnCreateModel model, ColumnType columnType, CancellationToken ct);
    public Task<List<ColumnVm>> UpdateColumn(int columnId, ColumnUpdateModel model, CancellationToken ct);
    public Task<List<ColumnVm>> MoveColumn(int columnId, ColumnMoveModel model, CancellationToken ct);
}