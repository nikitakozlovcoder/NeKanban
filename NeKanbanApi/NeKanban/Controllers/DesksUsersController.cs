using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Constants;
using NeKanban.Controllers.Models;
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

    public DesksUsersController(IDesksService desksService, IDeskUserService deskUserService, UserManager<ApplicationUser> userManager) : base(userManager)
    {
        _desksService = desksService;
        _deskUserService = deskUserService;
    }

    [HttpPut("{deskId:int}")]
    public Task<DeskVm> RemoveUsers([FromBody]DeskRemoveUsersModel deskRemoveUsersModel, int deskId, CancellationToken ct = default)
    {
        return _desksService.UpdateDesk(deskRemoveUsersModel, deskId, ct);
    }
    
    [HttpPut("{deskId:int}")]
    public async Task<List<DeskLightVm>> SetPreferenceType([FromBody]DeskUserUpdatePreferenceType preferenceType, int deskId, CancellationToken ct = default)
    {
        var user = await GetApplicationUser();
        return await _deskUserService.SetPreference(preferenceType, user, deskId, ct);
    }
}