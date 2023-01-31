using AutoMapper;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NeKanban.Common.AppMapper;
using NeKanban.Common.Attributes;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.Desks;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.ColumnModels;
using NeKanban.Common.Models.DeskModels;
using NeKanban.Common.ViewModels;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.SecurityProfile.Helpers;
using NeKanban.Logic.Services.Columns;
using NeKanban.Logic.Services.DesksUsers;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.Services.Desks;

[UsedImplicitly]
[Injectable<IDesksService>]
public class DesksService : BaseService, IDesksService
{
    private readonly IRepository<Desk> _deskRepository;
    private readonly IDeskUserService _deskUserService;
    private readonly IColumnsService _columnsService;
    private readonly IAppMapper _mapper;
    public DesksService(IRepository<Desk> deskRepository, 
        IDeskUserService deskUserService, 
        IColumnsService columnsService, 
        IAppMapper mapper) 
    {
        _deskRepository = deskRepository;
        _deskUserService = deskUserService;
        _columnsService = columnsService;
        _mapper = mapper;
    }
    
    public async Task<DeskVm> CreateDesk(DeskCreateModel deskCreateModel, ApplicationUser user, CancellationToken ct)
    {
        var desk = _mapper.Map<Desk, DeskCreateModel>(deskCreateModel);
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
        var desk = await _deskRepository.First(x => x.Id == id, ct);
        await _deskRepository.Remove(desk!, ct);
    }

    public async Task<DeskVm> GetDesk(int id, ApplicationUser user, CancellationToken ct)
    {
        var desk = await _deskRepository.ProjectToSingle<DeskDto>(x => x.Id == id, ct);
        var role = desk.DeskUsers.FirstOrDefault(x => x.UserId == user.Id)?.Role;
        var canViewInviteLink = role.HasValue && PermissionChecker.CheckPermission(role.Value, PermissionType.ViewInviteLink);
        var deskVm = _mapper.Map<DeskVm, DeskDto>(desk);
        deskVm.InviteLink = canViewInviteLink ? desk.InviteLink : null;
        return deskVm;

    }

    public async Task<DeskVm> UpdateDesk(DeskUpdateModel deskUpdateModel, int id, ApplicationUser user, CancellationToken ct)
    {
        var desk = await _deskRepository.Single(x => x.Id == id, ct);
        _mapper.Map(deskUpdateModel, desk);
        await _deskRepository.Update(desk!, ct);
        return await GetDesk(desk!.Id, user, ct);
    }

    public async Task<DeskVm> UpdateDesk(DeskInviteLinkModel inviteLinkModel, int id, ApplicationUser user, CancellationToken ct)
    {
        var desk = await _deskRepository.Single(x => x.Id == id, ct);
        desk.InviteLink = inviteLinkModel.Action switch
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
        var desk = await _deskRepository.First(x => x.InviteLink == model.Uid, ct);
        await _deskUserService.CreateDeskUser(desk.Id, user.Id, RoleType.User, ct);
        return await GetDesk(desk.Id, user, ct);
    }
}