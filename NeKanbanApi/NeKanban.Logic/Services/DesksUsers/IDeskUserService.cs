using NeKanban.Data.Entities;
using NeKanban.Logic.Models.DeskModels;
using NeKanban.Logic.Models.DeskUserModels;
using NeKanban.Logic.Services.ViewModels;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.Services.DesksUsers;

public interface IDeskUserService
{
    Task CreateDeskUser(int deskId, int userId, RoleType role, CancellationToken ct);
    Task<DeskUserVm> GetDeskUser(int id, CancellationToken ct);
    Task RemoveFromDesk(int userId, int id, CancellationToken ct);
    Task<List<DeskLightVm>> SetPreference(DeskUserUpdatePreferenceType preferenceType, ApplicationUser applicationUser, int deskId, CancellationToken ct);
    Task<List<DeskUserVm>> ChangeRole(DeskUserRoleChangeModel model, int deskUserId, CancellationToken ct);
    Task<int> GetDeskUserUserId(int deskUserId, CancellationToken ct);
}