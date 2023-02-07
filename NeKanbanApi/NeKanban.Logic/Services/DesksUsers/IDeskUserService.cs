using NeKanban.Common.DTOs.Desks;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.DeskModels;
using NeKanban.Common.Models.DeskUserModels;

namespace NeKanban.Logic.Services.DesksUsers;

public interface IDeskUserService
{
    Task CreateDeskUser(int deskId, int userId, bool isOwner, CancellationToken ct);
    Task<DeskUserDto> GetDeskUser(int id, CancellationToken ct);
    Task RemoveFromDesk(int userId, int id, CancellationToken ct);
    Task<List<DeskLiteDto>> SetPreference(DeskUserUpdatePreferenceType preferenceType, ApplicationUser applicationUser, int deskId, CancellationToken ct);
    Task<List<DeskUserDto>> ChangeRole(DeskUserRoleChangeModel model, int deskUserId, CancellationToken ct);
    Task<int> GetDeskUserUserId(int deskUserId, CancellationToken ct);
    Task<int> GetDeskUserId(int deskId, ApplicationUser user, CancellationToken ct);
}