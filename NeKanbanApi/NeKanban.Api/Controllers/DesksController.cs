using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Common.DTOs.Desks;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.DeskModels;
using NeKanban.Controllers.Auth;
using NeKanban.Logic.Services.Desks;
using NeKanban.Logic.Services.MyDesks;
using NeKanban.Security.Constants;

namespace NeKanban.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class DesksController : BaseAuthController
{
    private readonly IDesksService _desksService;
    private readonly IMyDesksService _myDesksService;
    public DesksController(IDesksService desksService, UserManager<ApplicationUser> userManager, 
        IMyDesksService myDesksService,
        IServiceProvider serviceProvider) : base(userManager, serviceProvider)
    {
        _desksService = desksService;
        _myDesksService = myDesksService;
    }
    
    [HttpPost]
    public async Task<DeskDto> CreateDesk([FromBody]DeskCreateModel deskCreateModel, CancellationToken ct = default)
    {
        return await _desksService.CreateDesk(deskCreateModel, await GetApplicationUser(), ct);
    }
    
    [HttpPut("{id:int}")]
    public async Task<DeskDto> UpdateDesk([FromBody]DeskUpdateModel deskUpdateModel, int id, CancellationToken ct = default)
    {
        await EnsureAbleTo<Desk>(PermissionType.UpdateGeneralDesk, id, ct);
        return await _desksService.UpdateDesk(deskUpdateModel, id, await GetApplicationUser(), ct);
    }
    
    [HttpPut("{id:int}")]
    public async Task<DeskDto> InviteLink([FromBody]DeskInviteLinkModel inviteLinkModel, int id, CancellationToken ct = default)
    {
        await EnsureAbleTo<Desk>(PermissionType.ManageInviteLink, id, ct);
        return await _desksService.UpdateDesk(inviteLinkModel, id, await GetApplicationUser(), ct);
    }
    
    [HttpGet("{id:int}")]
    public async Task<DeskDto> GetDesk(int id, CancellationToken ct = default)
    {
        await EnsureAbleTo<Desk>(id, ct);
        return await _desksService.GetDesk(id, await GetApplicationUser(), ct);
    }

    [HttpGet]
    public async Task<List<DeskLiteDto>> GetForUser(CancellationToken ct = default)
    {
        var user = await GetApplicationUser();
        return await _myDesksService.GetForUser(user.Id, ct);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        await EnsureAbleTo<Desk>(PermissionType.DeleteDesk, id, ct);
        await _desksService.DeleteDesk(id, ct);
        return Ok();
    }
}
