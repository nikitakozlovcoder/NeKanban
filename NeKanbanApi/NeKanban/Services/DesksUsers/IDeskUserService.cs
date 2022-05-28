using NeKanban.Constants;
using NeKanban.Controllers.Models;
using NeKanban.Controllers.Models.DeskModels;
using NeKanban.Controllers.Models.DeskUserModels;
using NeKanban.Data.Entities;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.DesksUsers;

public interface IDeskUserService
{
    Task CreateDeskUser(int deskId, int userId, RoleType role, CancellationToken ct);
    Task<DeskUserVm> GetDeskUser(int id, CancellationToken ct);
    Task RemoveFromDesk(int userId, int id, CancellationToken ct);
    Task<List<DeskLightVm>> SetPreference(DeskUserUpdatePreferenceType preferenceType, ApplicationUser applicationUser, int deskId, CancellationToken ct);
    Task<List<DeskUserVm>> ChangeRole(DeskUserRoleChangeModel model, int deskUserId, CancellationToken ct);
    Task<int> GetDeskUserUserId(int deskUserId, CancellationToken ct);
}