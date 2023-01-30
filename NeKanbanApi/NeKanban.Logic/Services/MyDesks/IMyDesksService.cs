using NeKanban.Common.ViewModels;

namespace NeKanban.Logic.Services.MyDesks;

public interface IMyDesksService
{
   Task<List<DeskLiteVm>> GetForUser(int userId, CancellationToken ct);
}