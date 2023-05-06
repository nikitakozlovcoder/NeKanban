using System.Net;
using Batteries.Exceptions;
using Batteries.Injection.Attributes;
using Batteries.Mapper.AppMapper;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.Desks;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.Exceptions;
using NeKanban.Common.Models.DeskModels;
using NeKanban.Common.Models.DeskUserModels;
using NeKanban.Data.Infrastructure.QueryFilters;
using NeKanban.Logic.Services.MyDesks;
using NeKanban.Logic.Services.Roles;

namespace NeKanban.Logic.Services.DesksUsers;

[UsedImplicitly]
[Injectable(typeof(IDeskUserService))]
public class DeskUserService : IDeskUserService
{
    private readonly IRepository<DeskUser> _deskUserRepository;
    private readonly IMyDesksService _myDesksService;
    private readonly IRolesService _rolesService;
    private readonly QueryFilterSettings _filterSettings;

    public DeskUserService(IRepository<DeskUser> deskUserRepository,
        IMyDesksService myDesksService,
        IAppMapper mapper,
        IRolesService rolesService,
        QueryFilterSettings filterSettings)
    {
        _deskUserRepository = deskUserRepository;
        _myDesksService = myDesksService;
        _rolesService = rolesService;
        _filterSettings = filterSettings;
    }

    public async Task CreateDeskUser(int deskId, int userId, bool isOwner, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            DeskUserDeletedFilter = false
        });
        
        var currDeskUser = await _deskUserRepository.SingleOrDefault(x => x.UserId == userId && x.DeskId == deskId, ct);
        if (currDeskUser != null)
        {
            switch (currDeskUser.DeletionReason)
            {
                case DeskUserDeletionReason.Exit:
                    currDeskUser.DeletionReason = null;
                    await _deskUserRepository.Update(currDeskUser, ct);
                    break;
                case DeskUserDeletionReason.Removed:
                    throw new HttpStatusCodeException(HttpStatusCode.Forbidden);
                case null:
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currDeskUser.DeletionReason));
            }
            
            return;
        }

        var deskUser = new DeskUser
        {
            DeskId = deskId,
            UserId = userId,
            IsOwner = isOwner,
            RoleId = null
        };

        if (!isOwner)
        {
            deskUser.RoleId = await _rolesService.GetDefaultRoleId(deskId, ct);
        }
        
        await _deskUserRepository.Create(deskUser, ct);
    }

    public async Task<DeskUserDto> GetDeskUser(int id, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.ProjectToSingle<DeskUserDto>(x => x.Id == id, ct);
        return deskUser;
    }

    public async Task RemoveFromDesk(int userId, int id, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            DeskUserDeletedFilter = false
        });
        
        var deskUser = await _deskUserRepository
            .Single(x=> x.UserId == userId && x.DeskId == id, ct);
        if (deskUser.IsOwner)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, Exceptions.CantRemoveOwnerFromDesk);
        }
        
        await _deskUserRepository.Remove(deskUser, ct);
    }

    public async Task Exit(int userId, int id, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository
            .Single(x=> x.UserId == userId && x.DeskId == id, ct);
        if (deskUser.IsOwner)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, Exceptions.CantRemoveOwnerFromDesk);
        }

        deskUser.DeletionReason = DeskUserDeletionReason.Exit;
        await _deskUserRepository.Update(deskUser, ct);
    }

    public async Task RevertRemovedDeskUser(int deskUserId, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            DeskUserDeletedFilter = false
        });
        
        var deskUser = await _deskUserRepository
            .Single(x=> x.Id == deskUserId, ct);

        if (deskUser.DeletionReason != DeskUserDeletionReason.Removed)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest);
        }

        deskUser.DeletionReason = null;
        await _deskUserRepository.Update(deskUser, ct);
    }

    public async Task<List<DeskUserDeletedDto>> GetDeletedUsers(int deskId, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            DeskUserDeletedFilter = false
        });

        var deletedUsers =
            await _deskUserRepository.ProjectTo<DeskUserDeletedDto>(
                x => x.DeletionReason.HasValue && x.DeskId == deskId, ct);
        
        return deletedUsers;
    }

    public async Task<List<DeskLiteDto>> SetPreference(DeskUserUpdatePreferenceType preferenceType, ApplicationUser applicationUser, int deskId,
        CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.First(x => x.UserId == applicationUser.Id && x.DeskId == deskId, ct);
        deskUser.Preference = preferenceType.Preference;
        await _deskUserRepository.Update(deskUser, ct);
        if (PreferenceTypeConstraints.ExclusivePreferenceTypes.Contains(preferenceType.Preference))
        {
            await ResetPreferences(PreferenceType.Favourite, deskUser.Id, ct);
        }

        return await _myDesksService.GetForUser(applicationUser.Id, ct);
    }

    public async Task<List<DeskUserDto>> ChangeRole(DeskUserRoleChangeModel model, int deskUserId, CancellationToken ct)
    {
        using var scope = _filterSettings.CreateScope(new QueryFilterSettingsDefinitions
        {
            DeskUserDeletedFilter = false
        });
        
        var deskUser = await _deskUserRepository.Single(x => x.Id == deskUserId, ct);
        if (deskUser.IsOwner)
        {
            throw new HttpStatusCodeException(HttpStatusCode.Unauthorized);
        }

        var role = await _rolesService.GetRole(model.RoleId, ct);
        if (role.DeskId != deskUser.DeskId)
        {
            throw new HttpStatusCodeException(HttpStatusCode.Unauthorized);
        }
        
        deskUser.RoleId = model.RoleId;
        await _deskUserRepository.Update(deskUser, ct);
        return await GetDeskUsers(deskUser.DeskId, ct);
    }

    public async Task<int> GetDeskUserUserId(int deskUserId, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.Single(x => x.Id == deskUserId ,ct);
        return deskUser.UserId;
    }

    public Task<int> GetDeskUserId(int deskId, ApplicationUser user, CancellationToken ct)
    {
        return _deskUserRepository.Single(x => x.UserId == user.Id && x.DeskId == deskId, x => x.Id, ct: ct);
    }

    private async Task ResetPreferences(PreferenceType type, int preserveId, CancellationToken ct)
    {
        var deskUsers = await _deskUserRepository.ToList(x => x.Preference == type && x.Id != preserveId, ct);
        foreach (var deskUser in deskUsers)
        {
            deskUser.Preference = PreferenceType.Normal;
            await _deskUserRepository.Update(deskUser, ct);
        }
    }

    private async Task<List<DeskUserDto>> GetDeskUsers(int deskId, CancellationToken ct)
    {
        var deskUsers = await _deskUserRepository.ProjectTo<DeskUserDto>(x => x.DeskId == deskId, ct);
        return deskUsers;
    }
}