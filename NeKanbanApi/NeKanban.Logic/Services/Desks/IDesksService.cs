using NeKanban.Common.Entities;
using NeKanban.Common.Models.DeskModels;
using NeKanban.Common.ViewModels;

namespace NeKanban.Logic.Services.Desks;

public interface IDesksService
{
    Task<DeskVm> CreateDesk(DeskCreateModel deskCreateModel, ApplicationUser user, CancellationToken ct);
    Task DeleteDesk(int id, CancellationToken ct);
    Task<DeskVm> GetDesk(int id, ApplicationUser user, CancellationToken ct);
    Task<DeskVm> UpdateDesk(DeskUpdateModel deskUpdateModel, int id, ApplicationUser user, CancellationToken ct);
    Task<DeskVm> UpdateDesk(DeskInviteLinkModel inviteLinkModel, int id, ApplicationUser user, CancellationToken ct);
    Task<DeskVm> UpdateDesk(DeskRemoveUsersModel deskRemoveUsersModel, int id, ApplicationUser user, CancellationToken ct);
    Task<DeskVm> AddUserToDesk(DeskAddUserByLinkModel model, ApplicationUser user, CancellationToken ct);
}