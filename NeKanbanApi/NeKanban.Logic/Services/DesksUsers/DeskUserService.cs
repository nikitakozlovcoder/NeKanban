using System.Net;
using Microsoft.EntityFrameworkCore;
using NeKanban.Api.FrameworkExceptions.ExceptionHandling;
using NeKanban.Common.Attributes;
using NeKanban.Data.Constants;
using NeKanban.Data.Entities;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Mappings;
using NeKanban.Logic.Models.DeskModels;
using NeKanban.Logic.Models.DeskUserModels;
using NeKanban.Logic.Services.ViewModels;
using NeKanban.Security.Constants;
using NeKanban.Services.MyDesks;

namespace NeKanban.Logic.Services.DesksUsers;

[Injectable(typeof(IDeskUserService))]
public class DeskUserService : BaseService, IDeskUserService
{
    private readonly IRepository<DeskUser> _deskUserRepository;
    private readonly IMyDesksService _myDesksService;

    public DeskUserService(IRepository<DeskUser> deskUserRepository, IMyDesksService myDesksService)
    {
        _deskUserRepository = deskUserRepository;
        _myDesksService = myDesksService;
    }

    public async Task CreateDeskUser(int deskId, int userId, RoleType role, CancellationToken ct)
    {
        var exists = await _deskUserRepository.QueryableSelect()
            .AnyAsync(x => x.UserId == userId && x.DeskId == deskId, ct);
        if (exists)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, Exceptions.UserAlreadyAddedToDesk);
        }
        await _deskUserRepository.Create(new DeskUser
        {
            DeskId = deskId,
            UserId = userId,
            Role = role
        }, ct);
    }

    public async Task<DeskUserVm> GetDeskUser(int id, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.QueryableSelect()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        if (deskUser == null)
        {
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, nameof(deskUser));
        }
        return deskUser.ToDeskUserVm();
    }

    public async Task RemoveFromDesk(int userId, int id, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository
            .FirstOrDefault(x=> x.UserId == userId && x.DeskId == id, ct);
        EnsureEntityExists(deskUser);
        if (deskUser!.Role == RoleType.Owner)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, Exceptions.CantRemoveOwnerFromDesk);
        }
      
        await _deskUserRepository.Remove(deskUser, ct);
    }

    public async Task<List<DeskLiteVm>> SetPreference(DeskUserUpdatePreferenceType preferenceType, ApplicationUser applicationUser, int deskId,
        CancellationToken ct)
    {
        var deskUser =
            await _deskUserRepository.FirstOrDefault(x => x.UserId == applicationUser.Id && x.DeskId == deskId, ct);
        EnsureEntityExists(deskUser);
        deskUser!.Preference = preferenceType.Preference;
        await _deskUserRepository.Update(deskUser, ct);
        if (PreferenceTypeConstraints.ExclusivePreferenceTypes.Contains(preferenceType.Preference))
        {
            await ResetPreferences(PreferenceType.Favourite, deskUser.Id, ct);
        }

        return await _myDesksService.GetForUser(applicationUser.Id, ct);
    }

    public async Task<List<DeskUserVm>> ChangeRole(DeskUserRoleChangeModel model, int deskUserId, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.QueryableSelect()
            .FirstOrDefaultAsync(x => x.Id == deskUserId, ct);
        EnsureEntityExists(deskUser);

        if (model.Role == RoleType.Owner)
        {
            throw new HttpStatusCodeException(HttpStatusCode.Unauthorized);
        }

        deskUser!.Role = model.Role;
        await _deskUserRepository.Update(deskUser, ct);
        return await GetDeskUsers(deskUser.DeskId, ct);
    }

    public async Task<int> GetDeskUserUserId(int deskUserId, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.QueryableSelect().FirstOrDefaultAsync(x => x.Id == deskUserId ,ct);
        EnsureEntityExists(deskUser);
        return deskUser!.UserId;
    }
    
    private async Task ResetPreferences(PreferenceType type, int preserveId, CancellationToken ct)
    {
        var deskUsers = await _deskUserRepository.QueryableSelect()
            .Where(x => x.Preference == type && x.Id != preserveId).ToListAsync(ct);
        
        foreach (var deskUser in deskUsers)
        {
            deskUser.Preference = PreferenceType.Normal;
            await _deskUserRepository.Update(deskUser, ct);
        }
    }

    private async Task<List<DeskUserVm>> GetDeskUsers(int deskId, CancellationToken ct)
    {
        var deskUsers = await _deskUserRepository.QueryableSelect()
            .Include(x=> x.User)
            .Where(x => x.DeskId == deskId).ToListAsync(ct);
        return deskUsers.Select(x => x.ToDeskUserVm()).ToList();

    }
}