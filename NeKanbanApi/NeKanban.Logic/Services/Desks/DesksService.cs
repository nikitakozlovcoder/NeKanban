using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeKanban.Common.Attributes;
using NeKanban.Data.Constants;
using NeKanban.Data.Entities;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Constants;
using NeKanban.Logic.Mappings;
using NeKanban.Logic.Models.ColumnModels;
using NeKanban.Logic.Models.DeskModels;
using NeKanban.Logic.SecurityProfile.Helpers;
using NeKanban.Logic.Services.Columns;
using NeKanban.Logic.Services.DesksUsers;
using NeKanban.Logic.Services.ViewModels;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.Services.Desks;

[Injectable<IDesksService>]
public class DesksService : BaseService, IDesksService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<Desk> _deskRepository;
    private readonly IDeskUserService _deskUserService;
    private readonly IColumnsService _columnsService;
    public DesksService(UserManager<ApplicationUser> userManager, 
        IHttpContextAccessor httpContextAccessor, 
        IRepository<Desk> deskRepository, IDeskUserService deskUserService, 
        IColumnsService columnsService) : base()
    {
        _httpContextAccessor = httpContextAccessor;
        _deskRepository = deskRepository;
        _deskUserService = deskUserService;
        _columnsService = columnsService;
    }
    
    public async Task<DeskVm> CreateDesk(DeskCreateModel deskCreateModel, ApplicationUser user, CancellationToken ct)
    {
        var desk = deskCreateModel.ToDesk();
        await _deskRepository.Create(desk, ct);
        await _deskUserService.CreateDeskUser(desk.Id, user.Id, RoleType.Owner, ct);
        await _columnsService.CreateColumn(desk.Id, new ColumnCreateModel
        {
            Name = ColumnNames.ToDo
        }, ColumnType.Start, ct);
        
        await _columnsService.CreateColumn(desk.Id, new ColumnCreateModel
        {
            Name = ColumnNames.Closed
        }, ColumnType.End, ct);
        return await GetDesk(desk.Id, user, ct);
    }

    public async Task DeleteDesk(int id, CancellationToken ct)
    {
        var desk = await _deskRepository.QueryableSelect().FirstOrDefaultAsync(x => x.Id == id, ct);
        EnsureEntityExists(desk);
        await _deskRepository.Remove(desk!, ct);
    }

    public async Task<DeskVm> GetDesk(int id, ApplicationUser user, CancellationToken ct)
    {
        var desk = await _deskRepository.QueryableSelect().Include(x => x.DeskUsers)
            .ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == id, ct);
        EnsureEntityExists(desk);
        var role = desk!.DeskUsers.FirstOrDefault(x => x.UserId == user.Id)?.Role;
        var canViewInviteLink = role.HasValue && PermissionChecker.CheckPermission(role.Value, PermissionType.ViewInviteLink);
        return desk.ToDeskVm(canViewInviteLink);

    }

    public async Task<DeskVm> UpdateDesk(DeskUpdateModel deskUpdateModel, int id, ApplicationUser user, CancellationToken ct)
    {
        var desk = await _deskRepository.QueryableSelect().FirstOrDefaultAsync(x => x.Id == id, ct);
        EnsureEntityExists(desk);
        desk!.FromUpdateModel(deskUpdateModel);
        await _deskRepository.Update(desk!, ct);
        return await GetDesk(desk!.Id, user, ct);
    }

    public async Task<DeskVm> UpdateDesk(DeskInviteLinkModel inviteLinkModel, int id, ApplicationUser user, CancellationToken ct)
    {
        var desk = await _deskRepository.QueryableSelect().FirstOrDefaultAsync(x => x.Id == id, ct);
        EnsureEntityExists(desk);
        desk!.InviteLink = inviteLinkModel.Action switch
        {
            InviteLinkAction.Remove => null,
            InviteLinkAction.Generate => Guid.NewGuid().ToString(),
            _ => throw new ArgumentOutOfRangeException()
        };
        await _deskRepository.Update(desk, ct);
        return await GetDesk(desk.Id, user, ct);
    }
    
    public async Task<DeskVm> UpdateDesk(DeskRemoveUsersModel deskRemoveUsersModel, int id, ApplicationUser user, CancellationToken ct)
    {
        foreach (var userId in deskRemoveUsersModel.UsersToRemove)
        {
            await _deskUserService.RemoveFromDesk(userId, id, ct);
        }
        return await GetDesk(id, user, ct);
    }

    public async Task<DeskVm> AddUserToDesk(DeskAddUserByLinkModel model, ApplicationUser user, CancellationToken ct)
    {
        var desk = await _deskRepository.QueryableSelect()
            .FirstOrDefaultAsync(x => x.InviteLink == model.Uid, ct);
        EnsureEntityExists(desk);
        await _deskUserService.CreateDeskUser(desk!.Id, user.Id, RoleType.User, ct);
        return await GetDesk(desk!.Id, user, ct);
    }
}