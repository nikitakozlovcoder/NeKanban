using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeKanban.Constants;
using NeKanban.Controllers.Models;
using NeKanban.Data;
using NeKanban.Data.Entities;
using NeKanban.ExceptionHandling;
using NeKanban.Mappings;
using NeKanban.Services.DesksUsers;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.Desks;

public class DesksService : BaseService, IDesksService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<Desk> _deskRepository;
    private readonly IDeskUserService _deskUserService;
    public DesksService(UserManager<ApplicationUser> userManager, 
        IHttpContextAccessor httpContextAccessor, 
        IRepository<Desk> deskRepository, IDeskUserService deskUserService) : base(userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _deskRepository = deskRepository;
        _deskUserService = deskUserService;
    }
    public async Task<DeskVm> CreateDesk(DeskCreateModel deskCreateModel, CancellationToken ct)
    {
        var desk = deskCreateModel.ToDesk();
        var user = await UserManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        await _deskRepository.Create(desk, ct);
        await _deskUserService.CreateDeskUser(desk.Id, user.Id, RoleType.Owner, ct);
        return await GetDesk(desk.Id, ct);
    }

    public Task<DeskVm> DeleteDesk(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
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
}