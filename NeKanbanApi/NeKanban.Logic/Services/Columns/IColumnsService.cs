using NeKanban.Common.Constants;
using NeKanban.Common.Models.ColumnModels;
using NeKanban.Common.ViewModels;

namespace NeKanban.Logic.Services.Columns;

public interface IColumnsService
{
    public Task<List<ColumnVm>> GetColumns(int deskId, CancellationToken ct);
    public Task<List<ColumnVm>> DeleteColumn(int columnId, CancellationToken ct);
    public Task<List<ColumnVm>> CreateColumn(int deskId, ColumnCreateModel model, ColumnType columnType, CancellationToken ct);
    public Task<List<ColumnVm>> UpdateColumn(int columnId, ColumnUpdateModel model, CancellationToken ct);
    public Task<List<ColumnVm>> MoveColumn(int columnId, ColumnMoveModel model, CancellationToken ct);
}