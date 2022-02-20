using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeKanban.Constants;
using NeKanban.Controllers.Models;
using NeKanban.Controllers.Models.ColumnModels;
using NeKanban.Controllers.Models.DeskModels;
using NeKanban.Data;
using NeKanban.Data.Entities;
using NeKanban.Mappings;
using NeKanban.Services.Columns;
using NeKanban.Services.DesksUsers;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.Desks;

public class DesksService : BaseService, IDesksService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<Desk> _deskRepository;
    private readonly IDeskUserService _deskUserService;
    private readonly IColumnsService _columnsService;
    public DesksService(UserManager<ApplicationUser> userManager, 
        IHttpContextAccessor httpContextAccessor, 
        IRepository<Desk> deskRepository, IDeskUserService deskUserService, 
        IColumnsService columnsService) : base(userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _deskRepository = deskRepository;
        _deskUserService = deskUserService;
        _columnsService = columnsService;
    }
    public async Task<DeskVm> CreateDesk(DeskCreateModel deskCreateModel, CancellationToken ct)
    {
        var desk = deskCreateModel.ToDesk();
        var user = await UserManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        await _deskRepository.Create(desk, ct);
        await _deskUserService.CreateDeskUser(desk.Id, user.Id, RoleType.Owner, ct);
        await _columnsService.CreateColumn(desk.Id, new ColumnCreateModel()
        {
            Name = ColumnNames.ToDo
        }, ColumnType.Start, ct);
        await _columnsService.CreateColumn(desk.Id, new ColumnCreateModel()
        {
            Name = ColumnNames.Closed
        }, ColumnType.End, ct);
        return await GetDesk(desk.Id, ct);
    }

    public async Task DeleteDesk(int id, CancellationToken ct)
    {
        var desk = await _deskRepository.QueryableSelect().FirstOrDefaultAsync(x => x.Id == id, ct);
        EnsureEntityExists(desk);
        await _deskRepository.Remove(desk!, ct);
    }

    public async Task<DeskVm> GetDesk(int id, CancellationToken ct)
    {
        var desk = await _deskRepository.QueryableSelect().Include(x => x.DeskUsers)
            .ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == id, ct);
        EnsureEntityExists(desk);
        var user = await UserManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        return desk!.ToDeskVm(user.Id);

    }
    
    public async Task<DeskVm> UpdateDesk(DeskUpdateModel deskUpdateModel, int id, CancellationToken ct)
    {
        var desk = await _deskRepository.QueryableSelect().FirstOrDefaultAsync(x => x.Id == id, ct);
        EnsureEntityExists(desk);
        desk!.FromUpdateModel(deskUpdateModel);
        await _deskRepository.Update(desk!, ct);
        return await GetDesk(desk!.Id, ct);
    }

    public async Task<DeskVm> UpdateDesk(DeskInviteLinkModel inviteLinkModel, int id, CancellationToken ct)
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
        return await GetDesk(desk.Id, ct);
    }
    
    public async Task<DeskVm> UpdateDesk(DeskRemoveUsersModel deskRemoveUsersModel, int id, CancellationToken ct)
    {
        foreach (var userId in deskRemoveUsersModel.UsersToRemove)
        {
            await _deskUserService.RemoveFromDesk(userId, id, ct);
        }
        return await GetDesk(id, ct);
    }

    public async Task<DeskVm> AddUserToDesk(DeskAddUserByLinkModel model, ApplicationUser user, CancellationToken ct)
    {
        var desk = await _deskRepository.QueryableSelect()
            .FirstOrDefaultAsync(x => x.InviteLink == model.Uid, ct);
        EnsureEntityExists(desk);
        await _deskUserService.CreateDeskUser(desk!.Id, user.Id, RoleType.User, ct);
        return await GetDesk(desk!.Id, ct);
    }
}