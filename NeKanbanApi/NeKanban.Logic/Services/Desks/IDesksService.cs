using NeKanban.Common.DTOs.Desks;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.DeskModels;

namespace NeKanban.Logic.Services.Desks;

public interface IDesksService
{
    Task<DeskDto> CreateDesk(DeskCreateModel deskCreateModel, ApplicationUser user, CancellationToken ct);
    Task DeleteDesk(int id, CancellationToken ct);
    Task<DeskDto> GetDesk(int id, ApplicationUser user, CancellationToken ct);
    Task<DeskDto> UpdateDesk(DeskUpdateModel deskUpdateModel, int id, ApplicationUser user, CancellationToken ct);
    Task<DeskDto> UpdateDesk(DeskInviteLinkModel inviteLinkModel, int id, ApplicationUser user, CancellationToken ct);
    Task<DeskDto> UpdateDesk(DeskRemoveUsersModel deskRemoveUsersModel, int id, ApplicationUser user, CancellationToken ct);
    Task<DeskDto> AddUserToDesk(DeskAddUserByLinkModel model, ApplicationUser user, CancellationToken ct);
}