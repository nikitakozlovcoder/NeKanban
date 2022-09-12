using NeKanban.Logic.Services.ViewModels;

namespace NeKanban.Services.MyDesks;

public interface IMyDesksService
{
   Task<List<DeskLiteVm>> GetForUser(int userId, CancellationToken ct);

}