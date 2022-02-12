using NeKanban.Constants;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.DesksUsers;

public interface IDeskUserService
{
    Task CreateDeskUser(int deskId, int userId, RoleType role, CancellationToken ct);
    Task<DeskUserVm> GetDeskUser(int id, CancellationToken ct);
}