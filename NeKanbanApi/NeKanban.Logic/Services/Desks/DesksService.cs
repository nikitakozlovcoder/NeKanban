using AutoMapper;
using JetBrains.Annotations;
using NeKanban.Common.AppMapper;
using NeKanban.Common.Attributes;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.Desks;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.ColumnModels;
using NeKanban.Common.Models.DeskModels;
using NeKanban.Common.ViewModels;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Services.Columns;
using NeKanban.Logic.Services.DesksUsers;
using NeKanban.Logic.Services.Roles;
using NeKanban.Logic.Services.Security;
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
    private readonly IPermissionCheckerService _permissionCheckerService;
    private readonly IRolesService _rolesService;
    public DesksService(IRepository<Desk> deskRepository, 
        IDeskUserService deskUserService, 
        IColumnsService columnsService, 
        IAppMapper mapper,
        IPermissionCheckerService permissionCheckerService,
        IRolesService rolesService) 
    {
        _deskRepository = deskRepository;
        _deskUserService = deskUserService;
        _columnsService = columnsService;
        _mapper = mapper;
        _permissionCheckerService = permissionCheckerService;
        _rolesService = rolesService;
    }
    
    public async Task<DeskDto> CreateDesk(DeskCreateModel deskCreateModel, ApplicationUser user, CancellationToken ct)
    {
        var desk = _mapper.AutoMap<Desk, DeskCreateModel>(deskCreateModel);
        await _deskRepository.Create(desk, ct);
        await _rolesService.CreateDefaultRoles(desk.Id, ct);
        await _deskUserService.CreateDeskUser(desk.Id, user.Id, true, ct);
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

    public async Task<DeskDto> GetDesk(int id, ApplicationUser user, CancellationToken ct)
    {
        var desk = await _deskRepository.ProjectToSingle<DeskDto>(x => x.Id == id, ct);
        var canViewInviteLink = await _permissionCheckerService.HasPermission(id, user.Id, PermissionType.ViewInviteLink, ct);
        desk.InviteLink = canViewInviteLink ? desk.InviteLink : null;
        return desk;
    }

    public async Task<DeskDto> UpdateDesk(DeskUpdateModel deskUpdateModel, int id, ApplicationUser user, CancellationToken ct)
    {
        var desk = await _deskRepository.Single(x => x.Id == id, ct);
        _mapper.AutoMap(deskUpdateModel, desk);
        await _deskRepository.Update(desk!, ct);
        return await GetDesk(desk!.Id, user, ct);
    }

    public async Task<DeskDto> UpdateDesk(DeskInviteLinkModel inviteLinkModel, int id, ApplicationUser user, CancellationToken ct)
    {
        var desk = await _deskRepository.Single(x => x.Id == id, ct);
        desk.InviteLink = inviteLinkModel.Action switch
        {
            InviteLinkAction.Remove => null,
            InviteLinkAction.Generate => Guid.NewGuid().ToString(),
            _ => throw new ArgumentOutOfRangeException(nameof(inviteLinkModel))
        };
        
        await _deskRepository.Update(desk, ct);
        return await GetDesk(desk.Id, user, ct);
    }
    
    public async Task<DeskDto> UpdateDesk(DeskRemoveUsersModel deskRemoveUsersModel, int id, ApplicationUser user, CancellationToken ct)
    {
        foreach (var userId in deskRemoveUsersModel.UsersToRemove)
        {
            await _deskUserService.RemoveFromDesk(userId, id, ct);
        }
        
        return await GetDesk(id, user, ct);
    }

    public async Task<DeskDto> AddUserToDesk(DeskAddUserByLinkModel model, ApplicationUser user, CancellationToken ct)
    {
        var desk = await _deskRepository.First(x => x.InviteLink == model.Uid, ct);
        await _deskUserService.CreateDeskUser(desk.Id, user.Id, false, ct);
        return await GetDesk(desk.Id, user, ct);
    }
}