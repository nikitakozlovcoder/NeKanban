using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Constants;
using NeKanban.Constants.Security;
using NeKanban.Controllers.Models;
using NeKanban.Controllers.Models.DeskModels;
using NeKanban.Controllers.Models.DeskUserModels;
using NeKanban.Data.Entities;
using NeKanban.Services.Desks;
using NeKanban.Services.DesksUsers;
using NeKanban.Services.ViewModels;

namespace NeKanban.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class DesksUsersController : BaseAuthController
{
    private readonly IDesksService _desksService;
    private readonly IDeskUserService _deskUserService;

    public DesksUsersController(
        IDesksService desksService, 
        IDeskUserService deskUserService, 
        UserManager<ApplicationUser> userManager,
        IServiceProvider serviceProvider) : base(userManager, serviceProvider)
    {
        _desksService = desksService;
        _deskUserService = deskUserService;
    }

    [HttpPut("{deskId:int}")]
    public async Task<DeskVm> RemoveUsers([FromBody]DeskRemoveUsersModel deskRemoveUsersModel, int deskId, CancellationToken ct = default)
    {
        await EnsureAbleTo<Desk>(PermissionType.RemoveUsers, deskId, ct);
        return await _desksService.UpdateDesk(deskRemoveUsersModel, deskId, ct);
    }
    
    [HttpPut("{deskId:int}")]
    public async Task<List<DeskLightVm>> SetPreferenceType([FromBody]DeskUserUpdatePreferenceType preferenceType, int deskId, CancellationToken ct = default)
    {
        var user = await GetApplicationUser();
        return await _deskUserService.SetPreference(preferenceType, user, deskId, ct);
    }
    
    [HttpPut]
    public async Task<DeskVm> AddUserByLink([FromBody]DeskAddUserByLinkModel model, CancellationToken ct = default)
    {
        var user = await GetApplicationUser();
        return await _desksService.AddUserToDesk(model, user, ct);
    }
    
    [HttpPut("{deskUserId:int}")]
    public async Task<List<DeskUserVm>> ChangeRole([FromBody]DeskUserRoleChangeModel model, int deskUserId, CancellationToken ct = default)
    {
        await EnsureAbleTo<DeskUser>(PermissionType.ChangeUserRole, deskUserId, ct);
        return await _deskUserService.ChangeRole(model, deskUserId, ct);
    }
}