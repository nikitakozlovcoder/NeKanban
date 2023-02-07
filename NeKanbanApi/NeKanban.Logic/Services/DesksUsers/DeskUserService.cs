﻿using System.Net;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NeKanban.Api.FrameworkExceptions.ExceptionHandling;
using NeKanban.Common.AppMapper;
using NeKanban.Common.Attributes;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.Desks;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.Exceptions;
using NeKanban.Common.Models.DeskModels;
using NeKanban.Common.Models.DeskUserModels;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Services.MyDesks;
using NeKanban.Logic.Services.Roles;

namespace NeKanban.Logic.Services.DesksUsers;

[UsedImplicitly]
[Injectable(typeof(IDeskUserService))]
public class DeskUserService : BaseService, IDeskUserService
{
    private readonly IRepository<DeskUser> _deskUserRepository;
    private readonly IMyDesksService _myDesksService;
    private readonly IAppMapper _mapper;
    private readonly IRolesService _rolesService;

    public DeskUserService(IRepository<DeskUser> deskUserRepository,
        IMyDesksService myDesksService,
        IAppMapper mapper,
        IRolesService rolesService)
    {
        _deskUserRepository = deskUserRepository;
        _myDesksService = myDesksService;
        _mapper = mapper;
        _rolesService = rolesService;
    }

    public async Task CreateDeskUser(int deskId, int userId, bool isOwner, CancellationToken ct)
    {
        var exists = await _deskUserRepository.QueryableSelect()
            .AnyAsync(x => x.UserId == userId && x.DeskId == deskId, ct);
        if (exists)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, Exceptions.UserAlreadyAddedToDesk);
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
        var deskUser = await _deskUserRepository
            .Single(x=> x.UserId == userId && x.DeskId == id, ct);
        if (deskUser.IsOwner)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, Exceptions.CantRemoveOwnerFromDesk);
        }
      
        await _deskUserRepository.Remove(deskUser, ct);
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