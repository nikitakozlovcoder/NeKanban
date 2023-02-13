using NeKanban.Common.DTOs.Desks;

namespace NeKanban.Logic.Services.MyDesks;

public interface IMyDesksService
{
   Task<List<DeskLiteDto>> GetForUser(int userId, CancellationToken ct);
}