using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.DeskModels;
using NeKanban.Common.Models.DeskUserModels;
using NeKanban.Common.ViewModels;
using NeKanban.Controllers.Auth;
using NeKanban.Logic.Services.Desks;
using NeKanban.Logic.Services.DesksUsers;
using NeKanban.Security.Constants;

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
        return await _desksService.UpdateDesk(deskRemoveUsersModel, deskId, await GetApplicationUser(), ct);
    }
    
    [HttpPut("{deskId:int}")]
    public async Task<List<DeskLiteVm>> SetPreferenceType([FromBody]DeskUserUpdatePreferenceType preferenceType, int deskId, CancellationToken ct = default)
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
