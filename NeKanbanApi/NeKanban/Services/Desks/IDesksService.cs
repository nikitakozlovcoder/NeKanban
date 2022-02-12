using NeKanban.Controllers.Models;
using NeKanban.Data.Entities;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.Desks;

public interface IDesksService
{
    Task<DeskVm> CreateDesk(DeskCreateModel deskCreateModel, CancellationToken ct);
    Task<DeskVm> DeleteDesk(int id, CancellationToken ct);
    Task<DeskVm> GetDesk(int id, CancellationToken ct);
    Task<DeskVm> UpdateDesk(int id, CancellationToken ct);
    Task<List<DeskVm>> GetForUser(int userId, CancellationToken ct);
}