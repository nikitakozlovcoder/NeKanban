using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeKanban.Constants;
using NeKanban.Controllers.Models;
using NeKanban.Controllers.Models.DeskModels;
using NeKanban.Data;
using NeKanban.Data.Entities;
using NeKanban.ExceptionHandling;
using NeKanban.Mappings;
using NeKanban.Services.MyDesks;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.DesksUsers;

public class DeskUserService : BaseService, IDeskUserService
{
    private readonly IRepository<DeskUser> _deskUserRepository;
    private readonly IMyDesksService _myDesksService;

    public DeskUserService(IRepository<DeskUser> deskUserRepository, UserManager<ApplicationUser> userManager, IMyDesksService myDesksService) : base(userManager)
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
            .GetFirstOrDefault(x=> x.UserId == userId && x.DeskId == id, ct);
        EnsureEntityExists(deskUser);
        if (deskUser!.Role == RoleType.Owner)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, Exceptions.CantRemoveOwnerFromDesk);
        }
      
        await _deskUserRepository.Remove(deskUser!, ct);
    }

    public async Task<List<DeskLightVm>> SetPreference(DeskUserUpdatePreferenceType preferenceType, ApplicationUser applicationUser, int deskId,
        CancellationToken ct)
    {
        var deskUser =
            await _deskUserRepository.GetFirstOrDefault(x => x.UserId == applicationUser.Id && x.DeskId == deskId, ct);
        EnsureEntityExists(deskUser);
        deskUser!.Preference = preferenceType.Preference;
        await _deskUserRepository.Update(deskUser, ct);
        if (PreferenceTypeConstraints.ExclusivePreferenceTypes.Contains(preferenceType.Preference))
        {
            await ResetPreferences(PreferenceType.Favourite, deskUser.Id, ct);
        }

        return await _myDesksService.GetForUser(applicationUser.Id, ct);
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
}