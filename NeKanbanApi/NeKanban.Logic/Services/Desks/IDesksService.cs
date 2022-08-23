using NeKanban.Data.Entities;
using NeKanban.Logic.Models.DeskModels;
using NeKanban.Logic.Services.ViewModels;

namespace NeKanban.Logic.Services.Desks;

public interface IDesksService
{
    Task<DeskVm> CreateDesk(DeskCreateModel deskCreateModel, CancellationToken ct);
    Task DeleteDesk(int id, CancellationToken ct);
    Task<DeskVm> GetDesk(int id, CancellationToken ct);
    Task<DeskVm> UpdateDesk(DeskUpdateModel deskUpdateModel, int id, CancellationToken ct);
    Task<DeskVm> UpdateDesk(DeskInviteLinkModel inviteLinkModel, int id, CancellationToken ct);
    Task<DeskVm> UpdateDesk(DeskRemoveUsersModel deskRemoveUsersModel, int id, CancellationToken ct);
    Task<DeskVm> AddUserToDesk(DeskAddUserByLinkModel model, ApplicationUser user, CancellationToken ct);
}